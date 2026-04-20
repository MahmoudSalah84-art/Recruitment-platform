using MediatR;

namespace Jobs.Domain.Common
{

	public abstract record DomainEvent : INotification
	{
		public DateTime OccurredOn { get; init; } = DateTime.UtcNow ;
		public string Id { get; init; } = string.Empty;

	}
}
