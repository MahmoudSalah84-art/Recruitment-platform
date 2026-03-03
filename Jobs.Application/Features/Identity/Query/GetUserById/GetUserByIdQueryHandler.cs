using Jobs.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Query.GetUserById
{



	// Get User By Id Handler
	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
	{
		private readonly ApplicationDbContext _context;

		public GetUserByIdQueryHandler(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			var user = await _context.Users
				.Include(u => u.UserRoles)
					.ThenInclude(ur => ur.Role)
				.Include(u => u.UserClaims)
				.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

			if (user == null)
				return Result<UserDto>.Failure("المستخدم غير موجود");

			var userDto = new UserDto
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				IsEmailVerified = user.IsEmailVerified,
				IsActive = user.IsActive,
				Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
				Claims = user.UserClaims.Select(uc => new ClaimDto
				{
					Type = uc.ClaimType,
					Value = uc.ClaimValue
				}).ToList()
			};

			return Result<UserDto>.Success(userDto);
		}
	}
