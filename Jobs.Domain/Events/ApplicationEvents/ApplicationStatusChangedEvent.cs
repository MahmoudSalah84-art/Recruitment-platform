using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Jobs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.ApplicationEvents
{
    public class ApplicationStatusChangedEvent : DomainEvent
    {
        public JobApplication Application { get; }
        public ApplicationStatus OldStatus { get; }
        public ApplicationStatus NewStatus { get; }

        public ApplicationStatusChangedEvent(JobApplication app, ApplicationStatus oldStatus, ApplicationStatus newStatus)
        {
            Application = app;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            OccurredOn = DateTime.UtcNow;
		}
    }

}
