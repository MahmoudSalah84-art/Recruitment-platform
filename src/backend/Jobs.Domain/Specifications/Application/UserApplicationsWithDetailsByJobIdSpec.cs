using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Specifications.Application
{
	public class UserApplicationsWithDetailsByJobIdSpec : BaseSpecifications<JobApplication>
	{
		public UserApplicationsWithDetailsByJobIdSpec(string jobId, int PageSize, int PageNumber)
			: base(a => a.JobId == jobId)
		{
			AddInclude(a => a.Job);
			AddInclude(a => a.Job.Company);
			AddOrderByDescending(a => a.CreatedAt);
			ApplyPagination(PageSize * (PageNumber - 1), PageSize);
		}
	}
}
