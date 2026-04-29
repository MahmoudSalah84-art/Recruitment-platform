using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Applications.Queries.GetApplicationById
{
    public record GetUserApplicationDetailsQuery(string CompanyId,string ApplicationId) : IQuery<GetUserApplicationDetailsDTO>
	{

    }
}
