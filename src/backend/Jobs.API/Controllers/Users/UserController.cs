using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Users.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Users
{
	public class UserController : ApiController
	{
		// Post: api/User/register
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
		{
			var result = await Sender.Send(command);

			var response = result.ToApiResponse();

			if (response.IsSuccess && response.Data is not null)
			{
				var accessToken = response.Data.AccessToken;
				var refreshToken = response.Data.RefreshToken;
				var accessTokenExpiry = response.Data.AccessTokenExpiry;

				// Access Token
				Response.Cookies.Append("accessToken", accessToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = false,
					SameSite = SameSiteMode.Strict,
					Expires = accessTokenExpiry
				});

				// Refresh Token
				Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = false,
					SameSite = SameSiteMode.Strict,
					Expires = DateTime.UtcNow.AddDays(7)
				});

				response.Data.AccessToken = " null";
				response.Data.RefreshToken = " null";
			}

			return StatusCode(response.StatusCode, response);
		}


	}
}
