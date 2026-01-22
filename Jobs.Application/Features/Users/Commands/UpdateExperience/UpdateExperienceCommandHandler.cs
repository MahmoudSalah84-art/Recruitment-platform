using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.UpdateExperience
{
	

	public class UpdateExperienceCommandHandler : IRequestHandler<UpdateExperienceCommand, Result>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public UpdateExperienceCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result> Handle(UpdateExperienceCommand request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

			// جلب الخبرة والتأكد من صاحبها
			var experience = await _unitOfWork.Experiences.GetByIdAsync(request.Id, cancellationToken);

			if (experience == null || experience.UserId != userId)
				return Result.Failure("Experience record not found or access denied.");

			experience.Title = request.Title;
			experience.Company = request.Company;
			experience.StartDate = request.StartDate;
			experience.EndDate = request.EndDate;

			_unitOfWork.Experiences.Update(experience);
			await _unitOfWork.CompleteAsync();

			return Result.Success();
		}
	}
}
