using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Applications.Queries.GetUserApplications;

namespace Jobs.Application.Features.Applications.Queries.GetApplicationsByJob
{
	public record GetApplicationsByJobQuery(
	string companyId,
	string JobId,
	int Page = 1,
	int PageSize = 10) : IQuery<PaginatedList<UserApplicationDTO>>;

}
