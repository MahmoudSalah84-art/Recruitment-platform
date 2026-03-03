using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.JobRoles
{
	public class JobExpirationDateMustBeFutureRule : IBusinessRule
	{
		private readonly DateTime _expiration;

		public JobExpirationDateMustBeFutureRule(DateTime expiration)
		{
			_expiration = expiration;
		}

		public string Message => "Job expiration date must be in the future.";

		public bool IsBroken() => _expiration <= DateTime.UtcNow;
	}

}
