using Jobs.Domain.Common;
using Jobs.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Jobs.Domain.Rules.UserRules
{
	public class EmailFormatRule : IBusinessRule
	{
		private readonly Email _email;

		public EmailFormatRule(Email email)
		{
			_email = email;
		}

		public bool IsBroken()
		{
			if (_email == null) return true;
			if (string.IsNullOrWhiteSpace(_email.Value)) return true;

			var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
			return !regex.IsMatch(_email.Value);
		}
		public string Message => "Email is invalid.";
	}

}
