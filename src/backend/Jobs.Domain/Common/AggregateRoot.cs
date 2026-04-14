
namespace Jobs.Domain.Common
{
	public abstract class AggregateRoot : BaseEntity 
	{

		// ===== Domain Events =====
		private readonly List<DomainEvent> _events = new();
		public IReadOnlyCollection<DomainEvent> Events => _events.AsReadOnly();


        protected void AddEvent(DomainEvent @event) => _events.Add(@event);
		public void ClearEvents() => _events.Clear();
	}
}