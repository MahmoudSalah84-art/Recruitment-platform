using Jobs.Domain.Entities;
using System.Security.Claims;

namespace Jobs.Application.Abstractions.Interfaces
{
	public interface IJwtTokenService
	{
		string GenerateAccessToken(User user, List<string> roles, List<Claim> claims);
		string GenerateRefreshToken();
		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
		bool ValidateToken(string token);
	}
}