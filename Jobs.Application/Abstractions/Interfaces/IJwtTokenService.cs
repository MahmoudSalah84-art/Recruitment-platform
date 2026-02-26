using Jobs.Domain.Entities;
using System.Security.Claims;

namespace Jobs.Application.Common.Interfaces
{
	public interface IJwtTokenService
	{
		string GenerateAccessToken(Guid userId, string email, IList<string> roles, IList<Claim> claims);
		string GenerateRefreshToken();
		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
		bool ValidateToken(string token);
	}
}