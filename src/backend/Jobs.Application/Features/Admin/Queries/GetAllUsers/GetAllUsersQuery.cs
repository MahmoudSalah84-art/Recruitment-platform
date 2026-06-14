using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Users.Queries.GetUserProfile;
using MediatR;


namespace Jobs.Application.Features.Admin.Queries.GetAllUsers
{
	public record GetAllUsersQuery(
	int PageNumber,
	int PageSize,
	string? SearchTerm,
	string? Role,
	bool? IsActive) : IQuery<PaginatedList<UserResponse>>;
}
