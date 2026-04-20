
namespace Jobs.Application.Common.DTOs
{
	public class AuthResponse
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime AccessTokenExpiry { get; set; }
		public string UserId { get; set; }
		public string Email { get; set; }
		public IEnumerable<string> Roles { get; set; }
		public IEnumerable<string> Permissions { get; set; }

		public AuthResponse()
		{
		}

		public AuthResponse(
			string accessToken,
			string refreshToken,
			DateTime accessTokenExpiry,
			string userId,
			string email,
			IEnumerable<string> roles,
			IEnumerable<string> permissions)
		{
			AccessToken = accessToken;
			RefreshToken = refreshToken;
			AccessTokenExpiry = accessTokenExpiry;
			UserId = userId;
			Email = email;
			Roles = roles;
			Permissions = permissions;
		}
	}
}
