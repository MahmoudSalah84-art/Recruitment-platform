using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Applications.Queries.GetApplicationById
{
    public record GetUserApplicationDetailsQuery(string companyId,string ApplicationId) : IQuery<GetUserApplicationDetailsDTO>
	{

    }
}
