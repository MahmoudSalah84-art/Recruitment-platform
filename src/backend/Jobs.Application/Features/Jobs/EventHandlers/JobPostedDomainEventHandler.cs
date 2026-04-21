using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation;
using Jobs.Domain.Events.CV_Recommendation_Events;
using Jobs.Domain.Events.JobEvents;
using MediatR;

namespace Jobs.Application.Features.Jobs.EventHandlers
{
	public sealed class JobPostedDomainEventHandler : INotificationHandler<JobPostedEvent>
	{
		private readonly ISender _sender;

		public JobPostedDomainEventHandler(ISender sender)
		{
			_sender = sender;
		}

		public async Task Handle(JobPostedEvent notification, CancellationToken cancellationToken)
		{
			await _sender.Send(
				new CreateCVJobRecommendationCommand_job(notification.Id),
				cancellationToken);
		}
	}
}