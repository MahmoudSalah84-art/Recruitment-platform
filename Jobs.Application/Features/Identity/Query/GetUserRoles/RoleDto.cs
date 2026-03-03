using Jobs.Application.Features.Identity.Query.GetUserClaims;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetUserRoles
{

	// Role DTOs
	public class RoleDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<ClaimDto> Claims { get; set; } = new();
	}

}
