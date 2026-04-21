using System;
using Jobs.Domain.Common;

namespace Jobs.Domain.Events.Company_Events
{
	public record CompanyCreatedEvent : DomainEvent
	{
		public CompanyCreatedEvent(string id) : base(id)
		{
		}
	}
}
