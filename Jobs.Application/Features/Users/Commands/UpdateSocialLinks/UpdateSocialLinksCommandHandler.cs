using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.UpdateSocialLinks
{
	public class UpdateSocialLinksCommandHandler : IRequestHandler<UpdateSocialLinksCommand, Result>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public UpdateSocialLinksCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result> Handle(UpdateSocialLinksCommand request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

			// جلب بيانات المستخدم الحالية
			var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
			if (user == null) return Result.Failure("User not found");

			// تحديث الروابط
			user.LinkedInUrl = request.LinkedInUrl;
			user.GitHubUrl = request.GitHubUrl;
			user.PortfolioUrl = request.PortfolioUrl;

			_unitOfWork.Users.Update(user);
			await _unitOfWork.CompleteAsync();

			return Result.Success();
		}
	}
}
