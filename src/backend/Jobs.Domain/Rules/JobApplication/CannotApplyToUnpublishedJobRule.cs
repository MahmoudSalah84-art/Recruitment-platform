using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.JobApplication
{

	public class CannotApplyToUnpublishedJobRule : IBusinessRule
	{
		private readonly Job _job;

		public CannotApplyToUnpublishedJobRule(Job job)
		{
			_job = job;
		}

		public bool IsBroken()
		{
			return !_job.IsPublished;
		}

		public string Message => "Cannot apply to an unpublished job.";
	}
}
