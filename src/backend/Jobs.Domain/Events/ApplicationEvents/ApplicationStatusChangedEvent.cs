using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Jobs.Domain.Enums;

namespace Jobs.Domain.Events.ApplicationEvents
{

	public record ApplicationStatusChangedEvent : DomainEvent
	{
		public ApplicationStatusChangedEvent(string ApplicationId)
		{
			Id = ApplicationId;
		}
	}
}
