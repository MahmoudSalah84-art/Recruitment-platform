using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Users.Queries.GetUserProfile
{
	public record GetUserByIdQuery(String UserId) : IQuery<UserResponse?>;
}
