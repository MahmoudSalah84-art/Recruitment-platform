using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using Jobs.Domain.Events.ApplicationEvents;
using Jobs.Domain.Exceptions;
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



		// Navigation Properties
		public User Applicant { get; set; }
		public Job Job { get; set; }
		public CV CV { get; set; }

		// ========= Constructors =========
		private JobApplication() { }

		public JobApplication(string applicantId, string jobId, int matchScore, string? cvId = null)
		{
			CheckRule(new NotEmptyGuidRule(applicantId));
			CheckRule(new NotEmptyGuidRule(jobId));
			if (matchScore < 0 || matchScore > 100)
				throw new DomainException("Match score must be between 0 and 100.");

			ApplicantId = applicantId;
			JobId = jobId;
			MatchScore = matchScore;
			CvId = cvId;


			Status = ApplicationStatus.Pending;

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

		// ==================== CV ====================
		public void AttachCV(string cvId)
		{

			if (Status != ApplicationStatus.Pending)
				throw new DomainException("Can only attach CV to a pending application.");

			CvId = cvId;
		}

		public void DetachCV()
		{
			if (CvId is null)
				throw new DomainException("No CV attached to this application.");

			if (Status != ApplicationStatus.Pending)
				throw new DomainException("Can only detach CV from a pending application.");

			CvId = null;
		}

		// ==================== Match Score ====================
		public void UpdateMatchScore(int newScore)
		{
			if (newScore < 0 || newScore > 100)
				throw new DomainException("Match score must be between 0 and 100.");

			if (Status != ApplicationStatus.Pending)
				throw new DomainException("Can only update match score for pending applications.");

			MatchScore = newScore;
		}
	}
}


