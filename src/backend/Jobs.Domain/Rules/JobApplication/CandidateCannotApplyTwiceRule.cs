using Jobs.Domain.Common;
using Jobs.Domain.Entities;

namespace Jobs.Domain.Rules.JobApplication
{
	public class CandidateCannotApplyTwiceRule : IBusinessRule
	{
		private readonly User _user;
		private readonly Job _job;

		public CandidateCannotApplyTwiceRule(User user, Job jobId)
		{
			_user = user;
			_job = jobId;
		}

		public bool IsBroken() =>
			_user.Applications.Any(a => a.JobId == _job.Id);
		public string Message => 
			"Candidate has already applied to this job.";
	}

}
