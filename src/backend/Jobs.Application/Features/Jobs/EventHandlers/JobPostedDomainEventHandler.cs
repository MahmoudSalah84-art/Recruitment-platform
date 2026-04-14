using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation;
using Jobs.Domain.Events.JobEvents;
using MediatR;

//namespace Jobs.Application.Features.Jobs.EventHandlers
//{
//	internal sealed class JobPostedDomainEventHandler : INotificationHandler<JobCreatedEvent>
//	{
//		private readonly IBackgroundJobService _backgroundJobService;

//		public JobPostedDomainEventHandler(IBackgroundJobService backgroundJobService)
//		{
//			_backgroundJobService = backgroundJobService;
//		}

//		public Task Handle(JobCreatedEvent notification, CancellationToken cancellationToken)
//		{
//			// Enqueue في الخلفية فوراً بدون await
//			_backgroundJobService.Enqueue(
//				new CreateCVJobRecommendationCommand(notification.JobId));

//			return Task.CompletedTask;
//		}
//	}
//}
