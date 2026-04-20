using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.JobEvents
{
	public record JobUpdatedEvent : DomainEvent
	{
		public JobUpdatedEvent(string JobId)
		{
			Id = JobId;
		}
	}
}
