using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.Enums;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Jobs.Application.Features.Experiences.Commands.AddUserExperience
{
	public class AddExperienceCommandHandler : ICommandHandler<AddExperienceCommand, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public AddExperienceCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result<Guid>> Handle(AddExperienceCommand request, CancellationToken cancellationToken)
		{
			var userIdClaim = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
				return Result<Guid>.Failure("Invalid or missing user identifier.");

			var experience = new UserExperience
				(
				userId,
				request.JobTitle,
				request.CompanyId,
				request.EmploymentType,
				request.StartDate,
				request.EndDate,
				request.IsCurrent,
				request.Description
				);

			await _unitOfWork.Experiences.AddAsync(experience, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<Guid>.Success(experience.Id);
		}
    }
}
