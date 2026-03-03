using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetRoleById
{

	// Get Role By Id Query
	public class GetRoleByIdQuery : IRequest<Result<RoleDto>>
	{
		public Guid RoleId { get; set; }
	}

}
