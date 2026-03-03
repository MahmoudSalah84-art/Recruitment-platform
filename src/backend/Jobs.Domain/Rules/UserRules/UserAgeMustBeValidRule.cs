using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.UserRules
{
	public class UserAgeMustBeValidRule : IBusinessRule
	{
		private readonly int _age;

		public UserAgeMustBeValidRule(int age)
		{
			_age = age;
		}

		public string Message => "Age must be between 16 and 60.";

		public bool IsBroken() => _age < 16 || _age > 60;
	}

}
