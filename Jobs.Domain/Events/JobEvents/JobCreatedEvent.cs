using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.JobEvents
{
	public class JobCreatedEvent : DomainEvent
	{
		public Job Job { get; }

        public JobCreatedEvent(Job job)
		{
			Job = job;
			OccurredOn = DateTime.UtcNow;
		}
	}
}
