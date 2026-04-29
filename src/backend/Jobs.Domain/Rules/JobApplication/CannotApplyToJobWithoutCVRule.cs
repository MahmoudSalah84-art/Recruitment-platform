using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.JobApplication
{
	public class CannotApplyToJobWithoutCVRule : IBusinessRule
	{
		private readonly User _user;

		public CannotApplyToJobWithoutCVRule(User user)
		{
			_user = user;
		}

		public bool IsBroken()
		{
			return _user.CV == null;
		}

		public string Message => "Cannot apply to a job without a CV.";
	}
}
