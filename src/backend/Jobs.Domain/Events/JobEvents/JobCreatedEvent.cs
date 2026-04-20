using Jobs.Domain.Common;

namespace Jobs.Domain.Events.JobEvents
{



	public record JobCreatedEvent : DomainEvent
	{
		public JobCreatedEvent(string JobId)
		{
			Id = JobId;
		}
	}
}
