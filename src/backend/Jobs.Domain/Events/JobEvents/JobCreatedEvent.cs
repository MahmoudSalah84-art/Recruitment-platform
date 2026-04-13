using Jobs.Domain.Common;

namespace Jobs.Domain.Events.JobEvents
{
	public class JobCreatedEvent : DomainEvent
	{
		public string JobId { get; }

        public JobCreatedEvent(string jobId)
		{
			JobId = jobId;
			OccurredOn = DateTime.UtcNow;
		}
	}
}
