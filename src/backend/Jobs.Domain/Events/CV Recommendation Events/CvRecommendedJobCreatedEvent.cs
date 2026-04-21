using Jobs.Domain.Common;

namespace Jobs.Domain.Events.CV_Recommendation_Events
{


	public record CvRecommendedJobCreatedEvent : DomainEvent
	{
		public CvRecommendedJobCreatedEvent(string RecommendationId) : base(RecommendationId)
		{
			{
			}
		}
	}
}
