using MediatR;

namespace Jobs.Domain.Common
{

	public abstract record DomainEvent() : INotification
	{
		//public DateTime OccurredOn { get; set; } = DateTime.UtcNow ;
		//public string Id { get; set; } 

		//protected DomainEvent(string Id)
		//{
		//	this.Id = Id ;
		//}

	}
}
