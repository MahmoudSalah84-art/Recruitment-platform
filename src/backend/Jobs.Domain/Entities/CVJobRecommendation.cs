using Jobs.Domain.Common;
using Jobs.Domain.Events.CV_Recommendation_Events;
using Jobs.Domain.Rules;


namespace Jobs.Domain.Entities
{
    public class CVJobRecommendation :AggregateRoot , ISoftDelete
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
		public void Deactivate()
		{
			if (!IsActive)
				return;

			IsActive = false;
			DeactivatedAt = DateTime.UtcNow;

			AddEvent(new CVRecommendationDeactivatedEvent(this));
		}

		void ISoftDelete.SoftDelete()
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}
	}

}
