using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.UserRules
{
	public class UserEmailMustBeUniqueRule : IBusinessRule
	{
		private readonly Func<string, bool> _emailExists;
		private readonly string _email;

		public UserEmailMustBeUniqueRule(Func<string, bool> emailExists, string email)
		{
			_emailExists = emailExists;
			_email = email;
		}

		public bool IsBroken() => _emailExists(_email);

		public string Message => "Email already exists.";
	}

}
