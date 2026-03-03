using Jobs.Application.Common.Interfaces;
using Jobs.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Jobs.Infrastructure.Services
{
	public class JwtTokenService : IJwtTokenService
	{
		private readonly JwtSettings _jwtSettings;

		public JwtTokenService(IOptions<JwtSettings> jwtSettings)
		{
			_jwtSettings = jwtSettings.Value;
		}


		public string GenerateAccessToken(string userId, string email, string FirstName, string LastName, IEnumerable<string> roles, IEnumerable<string> permissions)
		{
			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub,   userId),
				new(JwtRegisteredClaimNames.Email, email!),
				new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
				new("firstName", FirstName),
				new("lastName",  LastName),
			};
			foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

			foreach (var perm in permissions) claims.Add(new Claim("Permission", perm));

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		
		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[64];

			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			return Convert.ToBase64String(randomNumber);
		}


		
		
		public bool ValidateToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = _jwtSettings.Issuer,
				ValidAudience = _jwtSettings.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
		{
			var parameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,
				ValidIssuer = _jwtSettings.Issuer,
				ValidAudience = _jwtSettings.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey))
			};
			try
			{
				var principal = new JwtSecurityTokenHandler().ValidateToken(token, parameters, out var secToken);
				if (secToken is not JwtSecurityToken jwt ||
					!jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
						StringComparison.InvariantCultureIgnoreCase))
					return null;

				return principal;
			}
			catch { return null; }
		}


	}
}
