using Jobs.Domain.Common;

namespace Jobs.Domain.Events.Company_Events
{
	public record CompanyCreatedEvent(string Id) : DomainEvent(Id);
	 
}
