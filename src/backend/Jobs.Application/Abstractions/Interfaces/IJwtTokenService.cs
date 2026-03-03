using System.Security.Claims;

namespace Jobs.Application.Common.Interfaces
{
	public interface IJwtTokenService
	{
		string GenerateAccessToken(string userId, string email,string FirstName,string LastName , IEnumerable<string> roles, IEnumerable<string> permissions)
		string GenerateRefreshToken();
		ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
		bool ValidateToken(string token);

	}
}