using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Jobs.Infrastructure.Services
{
	public class JwtOptions
	{
		public string Issuer { get; set; } = "JobSite";
		public string Audience { get; set; } = "JobSiteClients";
		public string SecretKey { get; set; } = "replace_this_with_long_secret";
		public int ExpiryMinutes { get; set; } = 60;
	}

	public interface IJwtTokenGenerator
	{
		string GenerateToken(Guid userId, string email, IEnumerable<Claim>? additionalClaims = null);
	}

	public class JwtTokenGenerator : IJwtTokenGenerator
	{
		private readonly JwtOptions _options;
		private readonly SigningCredentials _signingCredentials;

		public JwtTokenGenerator(IOptions<JwtOptions> options)
		{
			_options = options.Value;
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
			_signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		}

		public string GenerateToken(Guid userId, string email, IEnumerable<Claim>? additionalClaims = null)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			if (additionalClaims != null)
				claims.AddRange(additionalClaims);

			var token = new JwtSecurityToken(
				issuer: _options.Issuer,
				audience: _options.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
				signingCredentials: _signingCredentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
