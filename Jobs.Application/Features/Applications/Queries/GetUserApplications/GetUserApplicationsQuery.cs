using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Applications.Queries.GetUserApplications;

namespace Jobs.Application.Features.Applications.Queries.GetMyApplications
{
	public record GetUserApplicationsQuery(int PageNumber = 1, int PageSize = 10) : IQuery<PaginatedList<UserApplicationDTO>>;
}