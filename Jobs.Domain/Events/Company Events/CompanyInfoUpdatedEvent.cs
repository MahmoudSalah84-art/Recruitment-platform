using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules
{
	public class CompanyInfoUpdatedEvent : DomainEvent
	{
		public Guid CompanyId { get; }
		

		public CompanyInfoUpdatedEvent(Guid companyId)
		{
			CompanyId = companyId;
			OccurredOn = DateTime.UtcNow;
		}
	}

}
