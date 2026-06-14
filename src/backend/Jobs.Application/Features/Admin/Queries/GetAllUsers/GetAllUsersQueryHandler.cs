using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;

namespace Jobs.Application.Features.Admin.Queries.GetAllUsers
{

	//public class GetAllUsersQueryHandler
	//	: IQueryHandler<GetAllUsersQuery, PaginatedList<UserResponse>>
	//{
	//	private readonly IIdentityService _identityService;

	//	public GetAllUsersQueryHandler(IIdentityService identityService)
	//	{
	//		_identityService = identityService;
	//	}

	//	public async Task<Result<PaginatedList<UserResponse>>> Handle(
	//		GetAllUsersQuery request,
	//		CancellationToken cancellationToken)
	//	{
	//		var result = await _identityService.GetUsersAsync(
	//			request.PageNumber,
	//			request.PageSize,
	//			request.SearchTerm,
	//			request.Role,
	//			request.IsActive,
	//			cancellationToken);

	//		return result;
	//	}
	}
//}