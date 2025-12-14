using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Exceptions
{
	public class BusinessRuleViolationException : Exception
	{
		public IBusinessRule BrokenRule { get; }
		public string Details { get; }

		public BusinessRuleViolationException(IBusinessRule brokenRule) : base(brokenRule.Message)
		{
			BrokenRule = brokenRule;
			Details = brokenRule.Message;
		}

		public override string ToString()
		{
			return $"{BrokenRule.GetType().Name}: {BrokenRule.Message}";
		}
	}
}
