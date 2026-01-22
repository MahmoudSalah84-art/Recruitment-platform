using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.GetMyApplications
{

	public class GetMyApplicationsQueryHandler : IRequestHandler<GetMyApplicationsQuery, Result<List<ApplicationResponse>>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public GetMyApplicationsQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result<List<ApplicationResponse>>> Handle(GetMyApplicationsQuery request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

			// جلب الطلبات مع بيانات الوظيفة المرتبطة (Include Job)
			var applications = await _unitOfWork.Applications.GetByUserIdAsync(userId, cancellationToken);

			var response = applications.Select(a => new ApplicationResponse
			{
				Id = a.Id,
				JobTitle = a.Job.Title,
				CompanyName = a.Job.Company.Name,
				Status = a.Status,
				AppliedAt = a.AppliedAt
			}).ToList();

			return Result.Success(response);
		}
	}
}
