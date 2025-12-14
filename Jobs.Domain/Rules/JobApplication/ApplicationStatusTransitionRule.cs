using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.JobApplication
{
	public class ApplicationStatusTransitionRule : IBusinessRule
	{
		private readonly ApplicationStatus _current;
		private readonly ApplicationStatus _next;

		public ApplicationStatusTransitionRule(ApplicationStatus current,ApplicationStatus next)
		{
			_current = current;
			_next = next;
		}

		public bool IsBroken()
		{
			return _current switch
			{
				ApplicationStatus.Pending =>
					!(_next is ApplicationStatus.Accepted
						or ApplicationStatus.Rejected),

				ApplicationStatus.Accepted =>
					true,

				ApplicationStatus.Rejected =>
					true,

				_ => true
			};
		}

		public string Message =>
			$"Invalid status transition from {_current} to {_next}.";
	}
}
