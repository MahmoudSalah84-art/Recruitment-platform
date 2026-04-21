using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using CVJobRecommendationEntity = Jobs.Domain.Entities.CVJobRecommendation;

namespace Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation
{
	public sealed class CreateCVJobRecommendationCommandHandler_cv : ICommandHandler<CreateCVJobRecommendationCommand_cv>
	{
		private readonly IAiScoringService _aiScoringService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<CreateCVJobRecommendationCommandHandler_cv> _logger;
		private readonly IFileService _fileService;

		public CreateCVJobRecommendationCommandHandler_cv(
			IAiScoringService aiScoringService,
			IUnitOfWork unitOfWork,
			ILogger<CreateCVJobRecommendationCommandHandler_cv> logger,
			IFileService fileService)
		{
			_aiScoringService = aiScoringService;
			_unitOfWork = unitOfWork;
			_logger = logger;
			_fileService = fileService;
		}

		public async Task<Result> Handle(CreateCVJobRecommendationCommand_cv request,
			CancellationToken cancellationToken)
		{



			var cv = await _unitOfWork.CVs.GetByUserIdAsync(request.UserId, cancellationToken);
			if (cv is null)
			{
				_logger.LogWarning("CV of user {UserId} not found for AI scoring.", request.UserId);
				return Result.Failure("CV not found.");
			}

			var jobs = await _unitOfWork.Jobs.GetAllActiveJobsAsync(cancellationToken);
			if (!jobs.Any())
			{
				_logger.LogInformation("No active jobs found to score CV {CvId} against.", cv.Id);
				return Result.Failure("No active jobs found.");
			}

			_logger.LogInformation( "Scoring CV {CvId} against {Count} jobs...", cv.Id, jobs.Count());

			var recommendations = new List<CVJobRecommendationEntity>();
			var fileStream = await _fileService.GetFileStreamFromUrlAsync(cv.FilePath.Value);
			foreach (var job in jobs)
			{
				//// reset stream position لكل request
				//cv.FileStream.Seek(0, SeekOrigin.Begin);

				var result = await _aiScoringService.MatchJobAsync(
					fileStream,
					job.Description + " " + job.Requirements ,
					cancellationToken);

				recommendations.Add(new CVJobRecommendationEntity(
					cvId: cv.Id,
					jobId: job.Id,
					score: (int)result.MatchScore));
			}

			await _unitOfWork.CVJobRecommendations.AddRangeAsync(recommendations, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			_logger.LogInformation( "Saved {Count} job recommendations for CV {CvId}.", recommendations.Count, cv.Id);

			return Result.Success();
		}
	}
}