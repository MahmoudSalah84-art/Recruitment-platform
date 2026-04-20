using Jobs.Domain.Common;


namespace Jobs.Domain.Events.UserEvents
{
	public record UserUpdatedProfileEvent : DomainEvent
	{
		public UserUpdatedProfileEvent(string UserId)
		{
			Id = UserId;
		}
	}
}
