using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.AddUserExperience
{
	public class AddExperienceCommandHandler : ICommandHandler<AddExperienceCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public AddExperienceCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result> Handle(AddExperienceCommand request, CancellationToken cancellationToken)
		{
			var userId = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null) return Result.Failure("Unauthorized");

			var experience = new UserExperience
			{
				UserId = Guid.Parse(userId),
				Title = request.Title,
				Company = request.Company,
				StartDate = request.StartDate,
				EndDate = request.EndDate
			};

			await _unitOfWork.Experiences.AddAsync(experience);
			await _unitOfWork.SaveAsync(); // حفظ التغييرات عبر UoW

			return Result.Success();
		}

       
    }
}
