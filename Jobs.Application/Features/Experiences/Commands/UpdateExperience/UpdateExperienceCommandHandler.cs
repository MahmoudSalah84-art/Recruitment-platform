using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Users.Queries.GetUserProfile;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jobs.Application.Features.Experiences.Commands.UpdateExperience
{
	public class UpdateExperienceCommandHandler : ICommandHandler<UpdateExperienceCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAcc;

		public UpdateExperienceCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContextAcc = httpContext;
		}

        public async Task<Result> Handle(UpdateExperienceCommand request, CancellationToken cancellationToken)
		{
			var userId = _httpContextAcc.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!Guid.TryParse(userId, out var userIdGuid))
				return Result<UserResponse?>.Failure("InvalidUserId");

			var experience = await _unitOfWork.Experiences.Query()
				.Include(e => e.Company)
				.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

			if (experience == null || experience.UserId != userIdGuid)
				return Result.Failure("Experience record not found or access denied.");

			experience.Update(request.JobTitle, request.CompanyId,request.EmploymentType, request.StartDate, request.EndDate, request.IsCurrent, request.Description);

			_unitOfWork.Experiences.Update(experience);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
