using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Identity.Query.GetAllRoles;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetUsersInRole
{

	// Get All Roles Handler
	public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Result<List<RoleDto>>>
	{
		private readonly ApplicationDbContext _context;

		public GetAllRolesQueryHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Result<List<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
		{
			var roles = await _context.Roles
				.Include(r => r.RoleClaims)
				.ToListAsync(cancellationToken);

			var roleDtos = roles.Select(r => new RoleDto
			{
				Id = r.Id,
				Name = r.Name,
				Description = r.Description,
				Claims = r.RoleClaims.Select(rc => new ClaimDto
				{
					Type = rc.ClaimType,
					Value = rc.ClaimValue
				}).ToList()
			}).ToList();

			return Result<List<RoleDto>>.Success(roleDtos);
		}
	}

}
