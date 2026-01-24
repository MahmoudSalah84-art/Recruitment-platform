using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Applications.Queries.GetApplicationById;
using MediatR;

namespace Jobs.Application.Features.Applications.Queries.GetMyApplications
{
	public record GetUserApplicationsQuery : IRequest<Result<List<UserApplicationDTO>>>;
}
