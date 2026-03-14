using Jobs.Domain.Common;
using Jobs.Domain.Events.CV_Recommendation_Events;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Rules;


namespace Jobs.Domain.Entities
{
    public class CVJobRecommendation :AggregateRoot  
	{

		// ======== Properties ========
		public string CvId { get; }

        public string JobId { get;  }

        public int Score { get; }

        public bool IsActive { get; private set; }
		public DateTime? DeactivatedAt { get; private set; }

		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }

		// Navigation Properties
		public CV CV { get; set; }
		public Job Job { get; set; }

		// ======== Constructors ========

		private CVJobRecommendation() { }

		public CVJobRecommendation(string cvId, string jobId, int score)
		{
			CheckRule(new ScoreRangeRule(score));
			CheckRule(new NotEmptyGuidRule(cvId));
			CheckRule(new NotEmptyGuidRule(jobId));

			CvId = cvId;
			JobId = jobId;
			Score = score;
			CreatedAt = DateTime.UtcNow;
			IsActive = true;
			UpdatedAt = CreatedAt;
			AddEvent(new CvRecommendedJobCreatedEvent(this));

		}


		// ======== Behaviors ========
		// ==================== Activation ====================
		public void Deactivate()
		{
			if (!IsActive)
				throw new DomainException("Recommendation is already inactive.");

			IsActive = false;
			DeactivatedAt = DateTime.UtcNow;

			AddEvent(new CVRecommendationDeactivatedEvent(this));

		}

		public void Reactivate()
		{
			if (IsActive)
				throw new DomainException("Recommendation is already active.");

			if (IsDeleted)
				throw new DomainException("Cannot reactivate a deleted recommendation.");

			IsActive = true;
			DeactivatedAt = null;

			//RaiseDomainEvent(new CVJobRecommendationReactivatedDomainEvent(Id, CvId, JobId));
		}

		public void Restore()
		{
			if (!IsDeleted)
				throw new DomainException("Recommendation is not deleted.");

			IsDeleted = false;
			DeletedAt = null;

			//RaiseDomainEvent(new CVJobRecommendationRestoredDomainEvent(Id, CvId, JobId));
		}
	}
}
