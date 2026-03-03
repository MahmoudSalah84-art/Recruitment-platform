using Jobs.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetAllUsers
{

	// Get All Users Handler
	public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserDto>>>
	{
		private readonly ApplicationDbContext _context;

		public GetAllUsersQueryHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Result<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
		{
			var query = _context.Users
				.Include(u => u.UserRoles)
					.ThenInclude(ur => ur.Role)
				.Include(u => u.UserClaims)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(request.SearchTerm))
			{
				query = query.Where(u =>
					u.Email.Contains(request.SearchTerm) ||
					u.UserName.Contains(request.SearchTerm) ||
					u.FirstName.Contains(request.SearchTerm) ||
					u.LastName.Contains(request.SearchTerm));
			}

			if (request.IsActive.HasValue)
			{
				query = query.Where(u => u.IsActive == request.IsActive.Value);
			}

			var users = await query
				.Skip((request.PageNumber - 1) * request.PageSize)
				.Take(request.PageSize)
				.ToListAsync(cancellationToken);

			var userDtos = users.Select(u => new UserDto
			{
				Id = u.Id,
				Email = u.Email,
				UserName = u.UserName,
				FirstName = u.FirstName,
				LastName = u.LastName,
				IsEmailVerified = u.IsEmailVerified,
				IsActive = u.IsActive,
				Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList(),
				Claims = u.UserClaims.Select(uc => new ClaimDto
				{
					Type = uc.ClaimType,
					Value = uc.ClaimValue
				}).ToList()
			}).ToList();

			return Result<List<UserDto>>.Success(userDtos);
		}
	}

}
