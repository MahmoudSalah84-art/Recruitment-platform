
namespace Jobs.Domain.Common
{
	public abstract class AggregateRoot : BaseEntity 
	{

		// ===== Domain Events =====
		private readonly List<DomainEvent> _events = new();
		public IReadOnlyCollection<DomainEvent> Events => _events.AsReadOnly();


        protected void AddEvent(DomainEvent @event) => _events.Add(@event);
		public void ClearEvents() => _events.Clear();





		public override bool Equals(object? obj)
		{
			if (obj is not BaseEntity other)
				return false;

			if (ReferenceEquals(this, other))
				return true;

			if (Id.Equals(default(Guid)) || other.Id.Equals(default(Guid)))
				return false;

			return Id.Equals(other.Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public static bool operator ==(AggregateRoot? left, AggregateRoot? right)
		{
			if (Equals(left, null))
				return Equals(right, null);

			return left.Equals(right);
		}

		public static bool operator !=(AggregateRoot left, AggregateRoot right)
		{
			return !(left == right);
		}
	}
}