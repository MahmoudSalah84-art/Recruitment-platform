using MediatR;

namespace Jobs.Domain.Common
{
	public abstract class DomainEvent : INotification
	{
		public DateTime OccurredOn { get; protected set; }

	}
}
