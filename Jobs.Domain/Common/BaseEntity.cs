using Jobs.Domain.Common;
using Jobs.Domain.Events;
using Jobs.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Jobs.Domain.Common
{
    public abstract class BaseEntity
	{
        public Guid Id { get; protected set; } = Guid.NewGuid();

		public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; protected set; }

		

		protected static void CheckRule(IBusinessRule rule)
		{
			if (rule.IsBroken())
				throw new BusinessRuleViolationException(rule);
		}


		public override bool Equals(object obj)
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

		public static bool operator ==(BaseEntity left, BaseEntity right)
		{
			if (Equals(left, null))
				return Equals(right, null);

			return left.Equals(right);
		}

		public static bool operator !=(BaseEntity left, BaseEntity right)
		{
			return !(left == right);
		}

		protected void Touch() => UpdatedAt = DateTime.UtcNow;
		


	}
}


