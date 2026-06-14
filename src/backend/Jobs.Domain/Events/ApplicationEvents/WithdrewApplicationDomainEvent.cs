using Jobs.Domain.Common;

namespace Jobs.Domain.Events.ApplicationEvents
{

	public record WithdrewApplicationDomainEvent : DomainEvent
	{
		public WithdrewApplicationDomainEvent(string ApplicationId) : base(ApplicationId)
		{
		}
	}
}
