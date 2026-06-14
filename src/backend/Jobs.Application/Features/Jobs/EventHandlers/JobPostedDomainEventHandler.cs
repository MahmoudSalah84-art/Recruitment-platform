using Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation;
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
			var result = await _sender.Send(new CreateCVJobRecommendationCommand_job(notification.Id), cancellationToken);

			if (result.IsFailure)
			{
				// إجبر الـ Outbox Processor إنه يعرف إن فيه مصيبة حصلت عشان ميعلمش عليها كـ True
				throw new Exception($"Failed to process CV recommendations: {result.Error}");
			}
		}
	}
}