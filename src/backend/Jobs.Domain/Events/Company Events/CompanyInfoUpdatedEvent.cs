using Jobs.Domain.Common;

namespace Jobs.Domain.Rules
{


	public record CompanyInfoUpdatedEvent : DomainEvent
	{
		public CompanyInfoUpdatedEvent(string EmployeeId)  
		{
		}
	}
}
