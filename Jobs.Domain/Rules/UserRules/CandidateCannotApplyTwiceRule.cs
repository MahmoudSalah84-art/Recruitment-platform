using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.UserRules
{
	public class CandidateCannotApplyTwiceRule : IBusinessRule
	{
		private readonly User _candidate;
		private readonly Guid _jobId;

		public CandidateCannotApplyTwiceRule(User candidate, Guid jobId)
		{
			_candidate = candidate ;
			_jobId = jobId;
		}

		public bool IsBroken() =>
			_candidate.Applications.Any(a => a.JobId == _jobId);
		public string Message => 
			"Candidate has already applied to this job.";
	}

}
