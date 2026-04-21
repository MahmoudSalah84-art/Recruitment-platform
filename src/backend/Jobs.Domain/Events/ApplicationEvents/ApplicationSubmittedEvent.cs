using Jobs.Domain.Common;


namespace Jobs.Domain.Events.ApplicationEvents
{


	public record ApplicationSubmittedEvent : DomainEvent
	{
		public ApplicationSubmittedEvent(string ApplicationId) : base(ApplicationId)
		{
		}
	}

}
