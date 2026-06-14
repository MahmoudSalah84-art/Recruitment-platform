using Jobs.Domain.Common;


namespace Jobs.Domain.Events.ApplicationEvents
{


	public record ApplicationSubmittedEvent(string Id) : DomainEvent(Id);
	
}
