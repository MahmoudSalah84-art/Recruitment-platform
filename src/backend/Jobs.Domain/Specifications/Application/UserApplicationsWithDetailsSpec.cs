using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Specifications.Application
{
	public class UserApplicationsWithDetailsSpec : BaseSpecifications<JobApplication>
	{
		public UserApplicationsWithDetailsSpec(string userId,int PageSize, int PageNumber)
			: base(a => a.ApplicantId == userId)
		{
			AddInclude(a => a.Job);
			AddInclude(a => a.Job.Company);
			AddOrderByDescending(a => a.CreatedAt);
			ApplyPagination(PageSize * (PageNumber - 1), PageSize);
		}
	}
}
