using Jobs.Domain.Common;
using Jobs.Domain.Events.CV_Recommendation_Events;
using Jobs.Domain.Rules;


namespace Jobs.Domain.Entities
{
    public class CVJobRecommendation :AggregateRoot
    {

		// ======== Properties ========
		public Guid CvId { get; }
        public CV CV { get; }

        public Guid JobId { get;  }
        public Job Job { get; }

        public int Score { get; }

        public bool IsActive { get; private set; }
		public DateTime? DeactivatedAt { get; private set; }

		// ======== Constructors ========

		private CVJobRecommendation() { }

		public CVJobRecommendation(Guid cvId, Guid jobId, int score)
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



		public void Deactivate()
		{
			if (!IsActive)
				return;

			IsActive = false;
			DeactivatedAt = DateTime.UtcNow;

			AddEvent(new CVRecommendationDeactivatedEvent(this));
		}

	}

}
