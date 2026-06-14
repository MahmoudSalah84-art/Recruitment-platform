using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
using Microsoft.Extensions.Logging;

namespace Jobs.Application.Features.Applications.Commands.RerankJobApplicantsJob
{
	public class RerankJobApplicantsJobCommandHandler : ICommandHandler<RerankJobApplicantsJobCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<RerankJobApplicantsJobCommandHandler> _logger;

		public RerankJobApplicantsJobCommandHandler(
		IUnitOfWork unitOfWork,
		ILogger<RerankJobApplicantsJobCommandHandler> logger
		)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}
		
		public async Task<Result> Handle(RerankJobApplicantsJobCommand request, CancellationToken cancellationToken)
		{
			var Application = await _unitOfWork.Applications.GetByIdAsync(request.ApplicationId, cancellationToken);
			if (Application is null)
			{
				_logger.LogWarning("Application {ApplicationId} not found for AI scoring.", request.ApplicationId);
				throw new Exception($"Application with ID {request.ApplicationId} not found.");
			}

			var JobRecommendation = await _unitOfWork.CVJobRecommendations.GetByCvIdAndJobIdAsync(Application.CvId!, Application.JobId, cancellationToken);
			if (JobRecommendation is null)
			{
				_logger.LogWarning("No job recommendation found for CV {CvId} and Job {JobId}.", Application.CvId, Application.JobId);
				throw new Exception($"No job recommendation found for CV {Application.CvId} and Job {Application.JobId}.");
			}

			Application.AddMatchScore(JobRecommendation.Score);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}