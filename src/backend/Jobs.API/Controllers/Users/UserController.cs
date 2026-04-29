using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Users.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Users
{
	public class UserController : ApiController
	{
		/// <summary>
		/// Registers a new user account and issues authentication tokens.
		/// </summary>
		/// <param name="command">The user registration data.</param>
		/// <param name="ct">Cancellation token for request cancellation.</param>
		/// <returns>
		/// Returns authentication tokens and user details upon successful registration.
		/// </returns>
		/// <remarks>
		/// - This endpoint creates a new user account.
		/// - Uses CQRS pattern via RegisterCommand.
		/// - On success, access and refresh tokens are stored in HttpOnly cookies.
		/// - Access token is short-lived, refresh token is valid for longer session renewal.
		/// </remarks>
		/// <response code="200">User registered successfully.</response>
		/// <response code="400">Invalid input data or registration failed.</response>
		/// <response code="409">User already exists.</response>
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
			}

			return StatusCode(response.StatusCode, response);
		}


	}
}
