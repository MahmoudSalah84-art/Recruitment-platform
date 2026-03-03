using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jobs.Application.Features.Experiences.Queries.GetUserExperiences
{
	public class GetUserExperiencesQueryHandler : IQueryHandler<GetUserExperiencesQuery, List<ExperienceDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAcc;

		public GetUserExperiencesQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContextAcc = httpContext;
		}

		public async Task<Result<List<ExperienceDto>>> Handle(GetUserExperiencesQuery request, CancellationToken cancellationToken)
		{
			var userIdClaim = _httpContextAcc.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
				return Result<List<ExperienceDto>>.Failure("Invalid or missing user identifier.");

			var experiences = await _unitOfWork.Experiences.Query()
				.Where(u => u.UserId == userId)
				.OrderByDescending(e => e.StartDate)
				.ToListAsync(cancellationToken);

			if (!experiences.Any())
				return Result<List<ExperienceDto>>.Failure("No experiences found for this user.");

			var dto = experiences.Select(ex => new ExperienceDto
			{
				Id = ex.Id,
				CompanyName = ex.Company.Name,
				StartDate = ex.StartDate,
				EndDate = ex.EndDate,
				IsCurrentRole = ex.IsCurrent,
				EmploymentType = ex.EmploymentType.ToString(),
				JobTitle = ex.JobTitle,
				Description = ex.Description,
			}).ToList();

			return Result<List<ExperienceDto>>.Success(dto);
		}
    }
}
