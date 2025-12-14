using Jobs.Domain.Common.YourProject.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.ValueObjects
{
	public sealed class PhoneNumber : ValueObject
	{
		public string Value { get; }

		private PhoneNumber(string value) => Value = value;

		public static PhoneNumber Create(string phone)
		{
			if (string.IsNullOrWhiteSpace(phone))
				throw new ArgumentException("Phone number is required.", nameof(phone));
			return new PhoneNumber(phone.Trim());
		}

		public override string ToString() => Value;
		public override bool Equals(object? obj) => obj is PhoneNumber other && Value == other.Value;
		public override int GetHashCode() => Value.GetHashCode();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
