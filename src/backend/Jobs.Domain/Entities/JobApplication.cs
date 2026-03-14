using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using Jobs.Domain.Events.ApplicationEvents;
using Jobs.Domain.Rules;
using Jobs.Domain.Rules.JobApplication;

namespace Jobs.Domain.Entities
{
    public class JobApplication : AggregateRoot
	{
		// ========= Properties =========

		public string ApplicantId { get; private set; }

		public string JobId { get; private set; }

		public string? CvId { get; private set; }

		public int MatchScore { get; private set; } // 0..100
		public ApplicationStatus Status { get; private set; } // Pending, Accepted, Rejected

		public DateTime StatusHistory { get; private set; } 
		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }



		// Navigation Properties
		public User Applicant { get; set; }
		public Job Job { get; set; }
		public CV CV { get; set; }

		// ========= Constructors =========
		private JobApplication() { }
		public JobApplication(string applicantId, string jobId, string? cvId)
		{
			CheckRule(new NotEmptyGuidRule(applicantId));
			CheckRule(new NotEmptyGuidRule(jobId));

			ApplicantId = applicantId;
			JobId = jobId;
			CvId = cvId;

			Status = ApplicationStatus.Pending;
			CreatedAt = DateTime.UtcNow;

			AddEvent(new ApplicationSubmittedEvent(this));
		}

		// ========= Behaviors =========

		public void ChangeStatus(ApplicationStatus newStatus)
		{
			CheckRule(new ApplicationStatusTransitionRule(Status, newStatus));

			var oldStatus = Status;
			Status = newStatus;
			StatusHistory = DateTime.UtcNow;

			AddEvent(new ApplicationStatusChangedEvent(this, oldStatus, newStatus));
		}

	}
}


