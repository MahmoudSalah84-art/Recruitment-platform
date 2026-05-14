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
				return Result<string>.Failure("Job not found.");
			}
			if (!job.IsPublished)
			{
				_logger.LogWarning("Job {JobId} is not published.", request.JobId);
				return Result<string>.Failure("Cannot recommend an unpublished job.");
			}
			if (job.IsExpired)
			{
				_logger.LogWarning("Job {JobId} is expired.", request.JobId);
				return Result<string>.Failure("Cannot recommend an expired job.");
			}

			var cvs = _unitOfWork.CVs.Query().ToList();
			if (!cvs.Any())
			{
				_logger.LogInformation("No CVs found to score for Job {JobId}.", request.JobId);
				return Result<string>.Failure("No CVs found.");
			}




			// get existing recommendations for the job to avoid re-scoring the same CVs
			var existingRecommendations = await _unitOfWork.CVJobRecommendations
				.GetByJobIdAsync(request.JobId, cancellationToken);

			var existingCvIds = existingRecommendations
				.Select(r => r.CvId)
				.ToHashSet();

			// filter the new CVs only
			var newCvs = cvs.Where(cv => !existingCvIds.Contains(cv.Id)).ToList();

			if (!newCvs.Any())
			{
				_logger.LogInformation("All CVs already scored for Job {JobId}. Skipping.", request.JobId);
				return Result<string>.Success("All CVs already scored.");
			}


			_logger.LogInformation("Scoring {Count} new CVs for Job {JobId}...", newCvs.Count, request.JobId);

			var recommendations = new List<CVJobRecommendationEntity>();

			foreach (var cv in newCvs)
			{
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

			_logger.LogInformation("Saved {Count} new CV recommendations for Job {JobId}.",
				recommendations.Count, request.JobId);

			return Result<string>.Success("Success");
		}
	}
}