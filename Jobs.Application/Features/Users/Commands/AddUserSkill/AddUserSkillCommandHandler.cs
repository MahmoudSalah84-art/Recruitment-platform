using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.AddUserSkill
{

	public class AddSkillCommandHandler : IRequestHandler<AddSkillCommand, Result>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public AddSkillCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result> Handle(AddSkillCommand request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

			// التحقق من أن المهارة غير موجودة مسبقاً لهذا المستخدم
			var exists = await _unitOfWork.Skills.AnyAsync(s => s.UserId == userId && s.Name == request.SkillName);
			if (exists) return Result.Failure("Skill already added.");

			var skill = new UserSkill { UserId = userId, Name = request.SkillName };
			await _unitOfWork.Skills.AddAsync(skill);
			await _unitOfWork.CompleteAsync();

			return Result.Success();
		}
	}
}
