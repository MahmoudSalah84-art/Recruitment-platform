using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Jobs.Queries.GetJobsWithPagination;
using Jobs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Jobs.Queries.SearchJobs
{
    public class JobsWithFiltersSpecificationQuery : IQuery<PaginatedList<JobDto>>
    {
		public int PageSize { get; set; } = 15;
		public int PageNumber { get; set; } = 1;
		public string? Search { get; set; }
		public string? Sort { get; set; }
		public string? Country { get; set; }
		public string? City { get; set; }
		public int? MinExperience { get; set; }
		public EmploymentType? Type { get; set; }
		public int? PostedInDays { get; set; }
	}
}