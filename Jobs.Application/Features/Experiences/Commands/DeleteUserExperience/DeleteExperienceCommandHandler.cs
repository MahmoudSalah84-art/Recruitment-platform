using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Users.Queries.GetUserProfile;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Jobs.Application.Features.Experiences.Commands.DeleteUserExperience
{
	public class DeleteExperienceCommandHandler : ICommandHandler<DeleteExperienceCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAcc;

		public DeleteExperienceCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContextAcc = httpContext;
		}

		public async Task<Result> Handle(DeleteExperienceCommand request, CancellationToken cancellationToken)
		{
			var userId = _httpContextAcc.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!Guid.TryParse(userId, out var userIdGuid))
				return Result<UserResponse?>.Failure("InvalidUserId");

			var experience = await _unitOfWork.Experiences
				.GetByIdAsync(request.Id, cancellationToken);

			if (experience == null || experience.UserId != userIdGuid)
				return Result.Failure("Experience record not found or access denied.");

			_unitOfWork.Experiences.Remove(experience);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
    }
}
