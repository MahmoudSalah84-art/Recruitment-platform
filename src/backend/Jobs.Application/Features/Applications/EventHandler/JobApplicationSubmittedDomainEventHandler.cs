using Jobs.Application.Features.Applications.Commands.RerankJobApplicantsJob;
using Jobs.Domain.Events.ApplicationEvents;
using MediatR;

namespace Jobs.Application.Features.Applications.EventHandler
{
	public sealed class JobApplicationSubmittedDomainEventHandler : INotificationHandler<ApplicationSubmittedEvent>
	{
		private readonly ISender _sender;

		public JobApplicationSubmittedDomainEventHandler(ISender sender)
		{
			_sender = sender;
		}

		public async Task Handle( ApplicationSubmittedEvent notification, CancellationToken cancellationToken)
		{
			await _sender.Send(
				new RerankJobApplicantsJobCommand(notification.Id), cancellationToken);
		}
	}
}
