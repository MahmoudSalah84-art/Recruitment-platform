using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules
{
	public class CompanyInfoUpdatedEvent : DomainEvent
	{
		public string CompanyId { get; }
		

		public CompanyInfoUpdatedEvent(string companyId)
		{
			CompanyId = companyId;
			OccurredOn = DateTime.UtcNow;
		}
	}

}
