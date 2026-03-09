using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Jobs.Queries.GetJobById;
using Jobs.Domain.Enums;


namespace Jobs.Application.Features.Jobs.Queries.SearchJobs
{
    public class JobsWithFiltersSpecificationQuery : IQuery<PaginatedList<JobResponse>>
    {
		public int PageSize { get; set; } = 10;
		public int PageNumber { get; set; } = 1;
		public string? Search { get; set; }
		public string? Sort { get; set; }
		public string? Country { get; set; }
		public string? City { get; set; }
		public int? MinExperience { get; set; }
		public EmploymentType? Type { get; set; }
		public int? PostedInDays { get; set; }
		public bool OnlyPublished { get; set; } = true;
	}
}