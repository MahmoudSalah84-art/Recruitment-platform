using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.CompanyRoles
{
	public class CompanyCannotPostMoreThanNJobsRule : IBusinessRule
	{
		private readonly Company _company;
		private readonly int _maxJobsAllowed;

		public CompanyCannotPostMoreThanNJobsRule(Company company, int maxJobsAllowed = 10)
		{
			_company = company ?? throw new ArgumentNullException(nameof(company));
			_maxJobsAllowed = maxJobsAllowed;
		}

		public bool IsBroken()
		{
			// Count only published or active jobs
			var activeJobsCount = _company.Jobs
				.Count(j => j.IsPublished && !j.IsExpired);

			return activeJobsCount >= _maxJobsAllowed;
		}

		public string Message =>
			$"Company cannot post more than {_maxJobsAllowed} active jobs.";
	}

}
