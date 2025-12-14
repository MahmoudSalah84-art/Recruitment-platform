using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules
{
	public class NotEmptyGuidRule : IBusinessRule
	{
		private readonly Guid _value;

		public NotEmptyGuidRule(Guid value)
		{
			_value = value;
		}

		public bool IsBroken()
		{
			return _value == Guid.Empty;
		}

		public string Message => "Guid cannot be empty.";
	}
}
