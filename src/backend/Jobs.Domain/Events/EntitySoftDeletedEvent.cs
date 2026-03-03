using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events
{
	public sealed class EntitySoftDeletedEvent : DomainEvent
	{
		public Guid EntityId { get; }

		public EntitySoftDeletedEvent(Guid id)
		{
			EntityId = id;
			OccurredOn = DateTime.UtcNow;
		}
	}
}
