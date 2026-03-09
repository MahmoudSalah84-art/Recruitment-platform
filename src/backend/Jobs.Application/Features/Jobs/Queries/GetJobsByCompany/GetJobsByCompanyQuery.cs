using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Jobs.Queries.GetJobById;
 
namespace Jobs.Application.Features.Jobs.Queries.GetJobsByCompany
{
	public record GetJobsByCompanyQuery(string CompanyId, int Page = 1, int PageSize = 10)
	: IQuery<PaginatedList<JobResponse>>;
}
