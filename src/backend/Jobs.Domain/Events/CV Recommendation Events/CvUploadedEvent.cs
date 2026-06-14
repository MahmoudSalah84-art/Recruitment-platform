using Jobs.Domain.Common;

namespace Jobs.Domain.Events.CV_Recommendation_Events
{
	public record CvUploadedEvent(string Id) : DomainEvent(Id);
}