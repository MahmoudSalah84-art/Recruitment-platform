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
				throw new Exception("CV not found for user.");
			}

			var jobs = await _unitOfWork.Jobs.GetAllActiveJobsAsync(cancellationToken);
			if (!jobs.Any())
			{
				_logger.LogInformation("No active jobs found to score CV {CvId} against.", cv.Id);
				throw new Exception("No active jobs found.");
			}

			// get existing recommendations for this CV to avoid re-scoring the same jobs
			var existingRecommendations = await _unitOfWork.CVJobRecommendations
				.GetByCvIdAsync(cv.Id, cancellationToken);

			// create a hash set of job IDs that already have recommendations for this CV 
			var existingJobIds = existingRecommendations
				.Select(r => r.JobId)
				.ToHashSet();

			// only score jobs that haven't been scored for this CV yet
			var newJobs = jobs.Where(j => !existingJobIds.Contains(j.Id)).ToList();

			if (!newJobs.Any())
			{
				_logger.LogInformation("All jobs already scored for CV {CvId}. Skipping.", cv.Id);
				return Result.Success();
			}

			_logger.LogInformation( "Scoring CV {CvId} against {Count} jobs...", cv.Id, jobs.Count());

			var recommendations = new List<CVJobRecommendationEntity>();
			var fileStream = await _fileService.GetFileStreamFromUrlAsync(cv.FilePath.Value);


			// 2. حول الملف بالكامل لـ Byte Array عشان يفضل معانا ثابت في الذاكرة
			using var tempMemoryStream = new MemoryStream();
			await fileStream.CopyToAsync(tempMemoryStream, cancellationToken);
			byte[] cvBytes = tempMemoryStream.ToArray();

			foreach (var job in newJobs)
			{
				// 3. في كل لفة، بنعمل Stream جديد طازة من الـ bytes
				// الـ using بتضمن إنه يتمسح بعد اللفة، وحتى لو الـ AI service قفلته مش هيأثر على اللفة الجاية
				using var chunkStream = new MemoryStream(cvBytes);


				var result = await _aiScoringService.MatchJobAsync(
					chunkStream,
					job.Description + " " + job.Requirements,
					cancellationToken);

				recommendations.Add(new CVJobRecommendationEntity(
					cvId: cv.Id,
					jobId: job.Id,
					score: (int)result.MatchScore));
			}

			await _unitOfWork.CVJobRecommendations.AddRangeAsync(recommendations, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			_logger.LogInformation("Saved {Count} new job recommendations for CV {CvId}.",
				recommendations.Count, cv.Id);

			return Result.Success();
		}
	}
}