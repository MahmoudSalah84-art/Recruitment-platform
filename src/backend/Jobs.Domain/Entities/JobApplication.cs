using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using Jobs.Domain.Events.ApplicationEvents;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Rules;
using Jobs.Domain.Rules.JobApplication;
using Jobs.Domain.Rules.UserRules;
using static System.Net.Mime.MediaTypeNames;

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

		public JobApplication(User applicant, Job job)
		{

		    CheckRule(new CandidateCannotApplyTwiceRule(applicant, job));
			CheckRule(new CannotApplyToExpiredJobRule(job));
			CheckRule(new CannotApplyToUnpublishedJobRule(job));
			CheckRule(new CannotApplyToJobWithoutCVRule(applicant));

			ApplicantId = applicant.Id;
			JobId = job.Id;
			CvId = applicant.CV?.Id;
			Status = ApplicationStatus.Pending;
			StatusHistory = DateTime.UtcNow;

			//AddEvent(new ApplicationSubmittedEvent(Id));
		}

		// ========= Behaviors =========

		public void AddMatchScore(int score)
		{
			if (score < 0 || score > 100)
				throw new DomainException("Match score must be between 0 and 100.");

			MatchScore = score;
			//AddEvent(new ApplicationMatchScoreUpdatedEvent(Id));
		}



		public void WithdrawApplication()
		{
			if (Status != ApplicationStatus.Pending)
				throw new DomainException("Cannot withdraw a processed application.");
			if (IsDeleted)
				throw new DomainException("Application already withdrawn");

			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;

			AddEvent(new WithdrewApplicationDomainEvent(Id));
		}


		public void ChangeStatus(ApplicationStatus newStatus)
		{
			CheckRule(new ApplicationStatusTransitionRule(Status, newStatus));

			var oldStatus = Status;
			Status = newStatus;
			StatusHistory = DateTime.UtcNow;

			AddEvent(new ApplicationStatusChangedEvent(Id));
		}
	}
}


