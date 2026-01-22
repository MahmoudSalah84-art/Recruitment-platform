using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.ApplyToJob
{
	public class ApplyToJobCommandHandler : IRequestHandler<ApplyToJobCommand, Result>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public ApplyToJobCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result> Handle(ApplyToJobCommand request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

			// التأكد أن المستخدم لم يتقدم لنفس الوظيفة سابقاً
			var alreadyApplied = await _unitOfWork.Applications.AnyAsync(a => a.JobId == request.JobId && a.UserId == userId);
			if (alreadyApplied) return Result.Failure("You have already applied for this job.");

			var application = new JobApplication
			{
				UserId = userId,
				JobId = request.JobId,
				AppliedAt = DateTime.UtcNow,
				Status = "Pending"
			};

			await _unitOfWork.Applications.AddAsync(application);
			await _unitOfWork.CompleteAsync();

			return Result.Success();
		}
	}
}
