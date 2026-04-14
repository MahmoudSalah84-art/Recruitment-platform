using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.CV_Recommendation_Events
{
    public class CvRecommendedJobCreatedEvent : DomainEvent
    {
		public string RecommendationId { get; }
		public string CvId { get; }
		public string JobId { get; }
		public double Score { get; }


        public CvRecommendedJobCreatedEvent(CVJobRecommendation rec)
        {
			RecommendationId = rec.Id;
			CvId = rec.CvId;
			JobId = rec.JobId;
			Score = rec.Score;
			OccurredOn = DateTime.UtcNow;
		}
	}


}
