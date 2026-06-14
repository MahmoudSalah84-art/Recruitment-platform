using Jobs.Domain.Common;

namespace Jobs.Domain.Events.JobEvents
{
	public record JobCreatedEvent(string JobId) : DomainEvent(JobId);
	
}
