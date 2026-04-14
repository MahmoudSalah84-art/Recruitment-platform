using Jobs.Domain.Events.JobEvents;
using MediatR;

namespace Jobs.Application.Features.Identity.EventHandler.GenerateEmailConfirmationTokenAfterRegister
{
	internal class GenerateEmailConfirmationTokenEventHandler : INotificationHandler<JobCreatedEvent>
	{
		public Task Handle(JobCreatedEvent notification, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
