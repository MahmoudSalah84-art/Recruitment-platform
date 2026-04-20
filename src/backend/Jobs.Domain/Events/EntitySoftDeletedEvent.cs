using Jobs.Domain.Common;

namespace Jobs.Domain.Events
{
	public record EntitySoftDeletedEvent : DomainEvent
	{
		public EntitySoftDeletedEvent(string EntityId)
		{
			Id = EntityId;
		}
	}
}
