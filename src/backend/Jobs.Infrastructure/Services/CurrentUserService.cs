using Jobs.Application.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace Jobs.Infrastructure.Services
{
	public class CurrentUserService : ICurrentUserService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CurrentUserService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string? UserId =>
			_httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

		public string? Email =>
			_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

		public bool IsAuthenticated =>
			_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

		public IEnumerable<string> Roles =>
			_httpContextAccessor.HttpContext?.User?
				.FindAll(ClaimTypes.Role)
				.Select(x => x.Value) ?? Enumerable.Empty<string>();
	}
}
