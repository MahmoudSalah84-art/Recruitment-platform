using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Jobs.Application.Features.Applications.Commands.WithdrawApplication
{

	public class WithdrawApplicationCommandHandler : IRequestHandler<WithdrawApplicationCommand, Result>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public WithdrawApplicationCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			var application = await _unitOfWork.Applications.GetByIdAsync(request.ApplicationId, cancellationToken);

			if (application == null || application.UserId != userId)
				return Result.Failure("Application not found.");

			// لا يمكن سحب الطلب إذا تم قبوله أو رفضه بالفعل (حسب منطق العمل)
			if (application.Status != "Pending")
				return Result.Failure("Cannot withdraw an application that has already been processed.");

			_unitOfWork.Applications.Remove(application);
			await _unitOfWork.CompleteAsync();

			return Result.Success();
		}
	}
}
