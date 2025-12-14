using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules
{
	public class NotEmptyRule : IBusinessRule
	{
		private readonly string _value;
		private readonly string _fieldName;

		public NotEmptyRule(string value, string fieldName)
		{
			_value = value;
			_fieldName = fieldName;
		}

		public string Message => $"{_fieldName} should not be empty.";

		public bool IsBroken() => string.IsNullOrWhiteSpace(_value);
	}

}
