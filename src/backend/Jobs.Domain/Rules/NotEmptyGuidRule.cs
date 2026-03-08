using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules
{
	public class NotEmptyGuidRule : IBusinessRule
	{
		private readonly string _value;

		public NotEmptyGuidRule(string value)
		{
			_value = value;
		}

		public bool IsBroken()
		{
			return _value == string.Empty;
		}

		public string Message => "Guid cannot be empty.";
	}
}
