using Jobs.Domain.Common;

namespace Jobs.Domain.Events.Company_Events
{

	public record CompanyEmployeeAddedDomainEvent : DomainEvent
	{
		public CompanyEmployeeAddedDomainEvent( string EmployeeId) : base(EmployeeId)
		{
		}
	}
}