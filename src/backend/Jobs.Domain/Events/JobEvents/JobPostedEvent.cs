using Jobs.Domain.Common;

namespace Jobs.Domain.Events.JobEvents
{
	public record JobPostedEvent(string Id) : DomainEvent(Id) ;
	
}
