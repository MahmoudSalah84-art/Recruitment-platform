using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using CVJobRecommendationEntity = Jobs.Domain.Entities.CVJobRecommendation;

namespace Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation
{
	public class CreateCVJobRecommendationCommandHandler_job : ICommandHandler<CreateCVJobRecommendationCommand_job, string>
	{
		private readonly IFileService _fileService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<CreateCVJobRecommendationCommandHandler_job> _logger;
		private readonly IAiScoringService _aiScoringService;
		public CreateCVJobRecommendationCommandHandler_job( IUnitOfWork unitOfWork ,
		ILogger<CreateCVJobRecommendationCommandHandler_job> logger,
		IAiScoringService aiScoringService ,
		IFileService fileService)
		{
			_aiScoringService = aiScoringService;
			_unitOfWork = unitOfWork;
			_logger = logger;
			_fileService = fileService;
		}

		public async Task<Result<string>> Handle(CreateCVJobRecommendationCommand_job request, CancellationToken cancellationToken)
		{
			var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);
			if (job is null)
			{
				_logger.LogWarning("Job {JobId} not found for AI scoring.", request.JobId);
				return Result<string>.Failure(" not found.");
			}
			//if (!job.IsPublished)
			//{
			//	_logger.LogWarning("Job {JobId} not found for AI scoring.", request.JobId);
			//	return Result<string>.Failure("Cannot recommend an unpublished job.");
			//}

			if (job.IsExpired)
			{
				_logger.LogWarning("Job {JobId} not found for AI scoring.", request.JobId);
				return Result<string>.Failure("Cannot recommend an expired job.");
			}

			var cvs = _unitOfWork.CVs.Query().ToList();

			if (!cvs.Any())
			{
				_logger.LogInformation("No CVs found to score for Job {JobId}.", request.JobId);
				return Result<string>.Failure("No CVs found .");
			}

			_logger.LogInformation("Scoring {Count} CVs for Job {JobId}...", cvs.Count, request.JobId);

			// Await all file-stream tasks and produce the required List<(string CvId, Stream CvPdf)>
			var cvStreamsArray = await Task.WhenAll(
				cvs.Select(async cv => (CvId: cv.Id, CvPdf: await _fileService.GetFileStreamFromUrlAsync(cv.FilePath.Value)))
			);

			var cvStreams = cvStreamsArray.ToList();

			var results = await _aiScoringService.RankCandidatesAsync(
								cvStreams,
								job.Description,
								cancellationToken);

			// احفظ الـ recommendations
			var recommendations = results.Select(result => new
				CVJobRecommendationEntity(
					cvId: result.CvId,
					jobId: job.Id,
					score: result.MatchScore))
				.ToList();

			await _unitOfWork.CVJobRecommendations.AddRangeAsync(recommendations);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<string>.Success("success");
		}
	}
}