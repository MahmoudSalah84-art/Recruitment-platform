using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Applications.Queries.GetApplicationById
{
    public record GetUserApplicationDetailsQuery(Guid Id) : IQuery<GetUserApplicationDetailsDTO>
	{

    }
}
