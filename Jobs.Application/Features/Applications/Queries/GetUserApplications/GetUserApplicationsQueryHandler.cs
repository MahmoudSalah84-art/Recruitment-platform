using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Applications.Queries.GetApplicationById;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Jobs.Application.Features.Applications.Queries.GetMyApplications
{

	public class GetUserApplicationsQueryHandler : IQueryHandler<GetUserApplicationsQuery, Result<List<GetUserApplicationDetailsDTO>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public GetUserApplicationsQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result<List<GetUserApplicationDetailsDTO>>> Handle(GetUserApplicationsQuery request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

			// جلب الطلبات مع بيانات الوظيفة المرتبطة (Include Job)
			var applications = await _unitOfWork.Applications.GetByUserIdAsync(userId, cancellationToken);

			var response = applications.Select(a => new GetUserApplicationDetailsDTO
			{
				Id = a.Id,
				JobTitle = a.Job.Title,
				CompanyName = a.Job.Company.Name,
				Status = a.Status,
				AppliedAt = a.AppliedAt
			}).ToList();

			return Result.Success(response);
		}

        Task<Result<Result<List<userApplicationDto>>>> IRequestHandler<GetUserApplicationsQuery, Result<Result<List<userApplicationDto>>>>.Handle(GetUserApplicationsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
