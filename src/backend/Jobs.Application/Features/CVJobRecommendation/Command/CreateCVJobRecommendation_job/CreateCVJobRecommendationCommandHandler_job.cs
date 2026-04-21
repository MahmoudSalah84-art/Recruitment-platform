using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
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
			if (!job.IsPublished)
			{
				_logger.LogWarning("Job {JobId} not found for AI scoring.", request.JobId);
				return Result<string>.Failure("Cannot recommend an unpublished job.");
			}

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

			var recommendations = new List<CVJobRecommendationEntity>();

			foreach (var cv in cvs)
			{
				//// reset stream position لكل request
				//cv.FileStream.Seek(0, SeekOrigin.Begin);

				var fileStream = await _fileService.GetFileStreamFromUrlAsync(cv.FilePath.Value);

				var result = await _aiScoringService.MatchJobAsync(
					fileStream,
					job.Description + " " + job.Requirements,
					cancellationToken);

				recommendations.Add(new CVJobRecommendationEntity(
					cvId: cv.Id,
					jobId: job.Id,
					score: (int)result.MatchScore));
			}

			await _unitOfWork.CVJobRecommendations.AddRangeAsync(recommendations);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<string>.Success("success");
		}
	}
}