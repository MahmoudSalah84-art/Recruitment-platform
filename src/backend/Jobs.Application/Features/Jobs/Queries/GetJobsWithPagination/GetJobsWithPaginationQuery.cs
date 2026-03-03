using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;

namespace Jobs.Application.Features.Jobs.Queries.GetJobsWithPagination
{
	public class GetJobsWithPaginationQuery : IQuery<PaginatedList<JobDto>>
	{
		public int PageNumber { get; set; } = 1; 
		public int PageSize { get; set; } = 10; 
	}
}
