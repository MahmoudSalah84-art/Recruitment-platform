
namespace Jobs.Application.Common.DTOs
{
	public record AuthResponse(
		string AccessToken,
		string RefreshToken,
		DateTime AccessTokenExpiry,
		string UserId,
		string Email,
		IEnumerable<string> Roles,
		IEnumerable<string> Permissions
	);
}
