using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Common;

namespace Jobs.Application.Features.Admin.Queries.GetUserDetails
{
	public record GetUserDetailsQuery(string UserId)
	: IQuery<UserDetailsResponse>;
}
