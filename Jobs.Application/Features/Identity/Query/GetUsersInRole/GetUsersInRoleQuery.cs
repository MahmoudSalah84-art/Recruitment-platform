using Jobs.Application.Abstractions.Messaging;
using MediatR;


namespace Jobs.Application.Features.Identity.Query.GetUsersInRole
{


	// Get Users In Role Query
	public class GetUsersInRoleQuery : IRequest<Result<List<UserDto>>>
	{
		public Guid RoleId { get; set; }
	}
}









