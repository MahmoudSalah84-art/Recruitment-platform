using Jobs.Application.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Jobs.Infrastructure.Identity.Services
{
	public class CurrentUserService : ICurrentUserService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CurrentUserService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public bool IsAuthenticated =>
			_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

		public Guid UserId
		{
			get
			{
				if (!IsAuthenticated)
					return Guid.Empty;

				var userIdClaim = _httpContextAccessor.HttpContext!.User
					.FindFirst(ClaimTypes.NameIdentifier)
					?? _httpContextAccessor.HttpContext.User.FindFirst("sub");

				return userIdClaim is null ? Guid.Empty : Guid.Parse(userIdClaim.Value);
			}
		}
	}
}
