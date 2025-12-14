using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules
{
    public class StringLengthRule : IBusinessRule
	{
		private readonly string _name;

		public StringLengthRule(string name)
		{
			_name = name;
		}

		public bool IsBroken() =>
			_name.Length < 2 || _name.Length > 50;

		public string Message =>
			"Field name length must be between 2 and 50 characters.";
	}
}
