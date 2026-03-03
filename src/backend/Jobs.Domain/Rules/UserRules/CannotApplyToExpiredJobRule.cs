using Jobs.Domain.Common;
using Jobs.Domain.Entities;

namespace Jobs.Domain.Rules.UserRules
{
	public class CannotApplyToExpiredJobRule : IBusinessRule
	{
		private readonly Job _job;

		public CannotApplyToExpiredJobRule(Job job)
		{
			_job = job;
		}

		public bool IsBroken()
		{
			return _job.ExpirationDate <= DateTime.UtcNow;
		}

		public string Message => "Cannot apply to an expired job.";
	}

}
