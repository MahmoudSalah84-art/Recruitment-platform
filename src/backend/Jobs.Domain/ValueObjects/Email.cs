using Jobs.Domain.Common.YourProject.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Jobs.Domain.ValueObjects
{
	public sealed class Email : ValueObject
	{
		public string Value { get; private set; }

		private Email(string value) => Value = value;

		public static Email Create(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("Email is required.", nameof(email));
			var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
			if (!regex.IsMatch(email))
				throw new ArgumentException("Invalid email format.", nameof(email));
			return new Email(email.Trim().ToLowerInvariant());
		}

		public override string ToString() => Value;
		public override bool Equals(object? obj) => obj is Email other && Value == other.Value;
		public override int GetHashCode() => Value.GetHashCode();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
