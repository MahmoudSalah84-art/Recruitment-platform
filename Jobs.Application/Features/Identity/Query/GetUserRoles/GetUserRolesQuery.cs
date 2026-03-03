using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetUserRoles
{

	// Get User Roles Query
	public class GetUserRolesQuery : IRequest<Result<List<RoleDto>>>
	{
		public Guid UserId { get; set; }
	}
}
