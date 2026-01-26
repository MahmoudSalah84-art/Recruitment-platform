using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Applications.Queries.GetUserApplications;
using Jobs.Domain.Specifications.Application;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Jobs.Application.Features.Applications.Queries.GetMyApplications
{

	public class GetUserApplicationsQueryHandler : IQueryHandler<GetUserApplicationsQuery, PaginatedList<UserApplicationDTO>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAcc;

		public GetUserApplicationsQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContextAcc = httpContext;
		}

		public async Task<Result<PaginatedList<UserApplicationDTO>>> Handle(GetUserApplicationsQuery request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContextAcc.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

			var spec = new UserApplicationsWithDetailsSpec(userId , request.PageSize ,request.PageNumber);

			var applications = await _unitOfWork.Applications.ListWithSpecAsync(spec);
			var Count = await _unitOfWork.Applications.CountAsync(spec);

			var paginatedList = new PaginatedList<UserApplicationDTO>(
				applications.Select(a => new UserApplicationDTO
				{
					Id = a.Id,
					JobTitle = a.Job.Title,
					CompanyName = a.Job.Company.Name,
					Status = a.Status.ToString(),
					AppliedAt = a.CreatedAt
				}).ToList(),
				Count,
				request.PageNumber,
				request.PageSize);
 
			return Result<PaginatedList<UserApplicationDTO>>.Success(paginatedList);
		}
 
    }
}
