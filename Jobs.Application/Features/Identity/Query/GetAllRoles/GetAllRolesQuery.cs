using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetAllRoles
{

	// Get All Roles Query
	public class GetAllRolesQuery : IRequest<Result<List<RoleDto>>>
	{
	}
}
