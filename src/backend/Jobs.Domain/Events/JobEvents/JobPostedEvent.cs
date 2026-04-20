using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.JobEvents
{


	public record JobPostedEvent : DomainEvent
	{
		public JobPostedEvent(string JobId)
		{
			Id = JobId;
		}
	}
}
