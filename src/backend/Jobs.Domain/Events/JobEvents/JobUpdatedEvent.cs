using Jobs.Domain.Common;


namespace Jobs.Domain.Events.JobEvents
{
	public record JobUpdatedEvent : DomainEvent
	{
		public JobUpdatedEvent(string JobId)  
		{
		}
	}
}
