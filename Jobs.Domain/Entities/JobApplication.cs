using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using Jobs.Domain.Events.ApplicationEvents;
using Jobs.Domain.Rules;
using Jobs.Domain.Rules.JobApplication;
using Jobs.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Entities
{
    public class JobApplication : AggregateRoot
    {
		// ========= Properties =========

		public Guid ApplicantId { get; private set; }
		public User Applicant { get; private set; }

		public Guid JobId { get; private set; }
		public Job Job { get; private set; }

		public Guid? CvId { get; private set; }
		public CV CV { get; private set; }

		public int MatchScore { get; private set; } // 0..100
		public ApplicationStatus Status { get; private set; }

		public StatusHistory StatusHistory { get; private set; } //oldStatus , newStatus , timestamp



		// ========= Constructors =========
		private JobApplication() { }
		public JobApplication(Guid applicantId, Guid jobId, Guid? cvId, int matchScore)
		{
			CheckRule(new NotEmptyGuidRule(applicantId));
			CheckRule(new NotEmptyGuidRule(jobId));
			CheckRule(new ScoreRangeRule(matchScore));

			ApplicantId = applicantId;
			JobId = jobId;
			CvId = cvId;
			MatchScore = matchScore;

			Status = ApplicationStatus.Pending;
			CreatedAt = DateTime.UtcNow;

			AddEvent(new ApplicationSubmittedEvent(this));
		}

		// ========= Behaviors =========

		public void ChangeStatus(ApplicationStatus newStatus, StatusHistory? history = null)
		{
			CheckRule(new ApplicationStatusTransitionRule(Status, newStatus));

			var oldStatus = Status;
			Status = newStatus;
			StatusHistory = history ?? StatusHistory.Create(oldStatus, newStatus);

			Touch();
			AddEvent(new ApplicationStatusChangedEvent(this, oldStatus, newStatus));
		}	
	}
}


