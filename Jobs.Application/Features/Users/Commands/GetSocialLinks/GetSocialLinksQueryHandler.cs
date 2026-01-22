using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.GetSocialLinks
{

	public class GetSocialLinksQueryHandler : IRequestHandler<GetSocialLinksQuery, Result<SocialLinksResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public GetSocialLinksQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result<SocialLinksResponse>> Handle(GetSocialLinksQuery request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);

			if (user == null) return Result.Failure<SocialLinksResponse>("User not found.");

			var response = new SocialLinksResponse
			{
				LinkedInUrl = user.LinkedInUrl,
				GitHubUrl = user.GitHubUrl,
				PortfolioUrl = user.PortfolioUrl
			};

			return Result.Success(response);
		}
	}
}
