using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.Company_Events
{


	public record CompanyEmployeeRemovedDomainEvent : DomainEvent
	{
		public CompanyEmployeeRemovedDomainEvent(string EmployeeId) : base(EmployeeId)
		{
		}
	}

}
