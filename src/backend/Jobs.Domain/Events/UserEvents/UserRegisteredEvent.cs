using Jobs.Domain.Common;

namespace Jobs.Domain.Events.Events
{


	public record UserRegisteredEvent : DomainEvent
	{
		public UserRegisteredEvent(string UserId)
		{
			Id = UserId;
		}
	}

}
