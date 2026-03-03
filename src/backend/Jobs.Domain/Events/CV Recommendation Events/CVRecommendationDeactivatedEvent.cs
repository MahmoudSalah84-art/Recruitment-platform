using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.CV_Recommendation_Events
{
	public class CVRecommendationDeactivatedEvent : DomainEvent
	{
		public Guid RecommendationId { get; }

		public CVRecommendationDeactivatedEvent(CVJobRecommendation rec)
		{
			RecommendationId = rec.Id;
			OccurredOn = DateTime.UtcNow;
		}
	}
}
