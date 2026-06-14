using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Applications.Queries.GetUseAppForSeekerById
{


	public record GetUserApplicationForSeekerQuiery(string userId, string ApplicationId) : IQuery<GetUserApplicationForSeekerDTO>
	{

	}
}
