using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Identity.Query.GetAllRoles;
using Jobs.Application.Features.Identity.Query.GetUsersInRole;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetUserRoles
{

	// Get User Roles Handler
	public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, Result<List<RoleDto>>>
	{
		private readonly ApplicationDbContext _context;

		public GetUserRolesQueryHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Result<List<RoleDto>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
		{
			var userRoles = await _context.UserRoles
				.Include(ur => ur.Role)
					.ThenInclude(r => r.RoleClaims)
				.Where(ur => ur.UserId == request.UserId)
				.Select(ur => ur.Role)
				.ToListAsync(cancellationToken);

			var roleDtos = userRoles.Select(r => new RoleDto
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
