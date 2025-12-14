using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.ApplicationEvents
{
    public class ApplicationSubmittedEvent : DomainEvent
    {
        public JobApplication Application { get; }

        public ApplicationSubmittedEvent(JobApplication application)
        {
            Application = application;
            OccurredOn = DateTime.UtcNow;

		}
    }

}
