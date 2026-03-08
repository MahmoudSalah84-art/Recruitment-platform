using Jobs.Domain.Exceptions;

namespace Jobs.Domain.Common
{
    public abstract class BaseEntity
	{
        public string Id { get; protected set; } = Guid.NewGuid().ToString();

		public DateTime CreatedAt { get; protected set; } 
		public DateTime? UpdatedAt { get; protected set; }

		

		protected static void CheckRule(IBusinessRule rule)
		{
			if (rule.IsBroken())
				throw new BusinessRuleViolationException(rule);
		}


		
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

		public static bool operator ==(BaseEntity? left, BaseEntity? right)
		{
			if (Equals(left, null))
				return Equals(right, null);

			return left.Equals(right);
		}

		public static bool operator !=(BaseEntity left, BaseEntity right)
		{
			return !(left == right);
		}
	}
}


