using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Identity.Command.ChangePassword;
using Jobs.Application.Features.Identity.Command.ConfirmEmail;
using Jobs.Application.Features.Identity.Command.ForgotPassword;
using Jobs.Application.Features.Identity.Command.GenerateEmailConfirmationToken;
using Jobs.Application.Features.Identity.Command.RefreshToken;
using Jobs.Application.Features.Identity.Command.ResetPassword;
using Jobs.Application.Features.Users.Commands.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Auth
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ApiController
	{
		// http://localhost:5000/api/auth/refresh-token
		[HttpPost("refresh-token")]
		[AllowAnonymous]
		public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
		{
			var result = await Sender.Send(command);
		
			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		//[HttpPost("logout")]
		//[Authorize]
		//public async Task<IActionResult> Logout()
		//{
		//	var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		//	var result = await Sender.Send(new LogoutCommand(userId!));

		//	var response = result.ToApiResponse();

		//	return StatusCode(response.StatusCode, response);
		//}

		// http://localhost:5000/api/auth/confirm-email?userId=123&token=abc
		[HttpGet("confirm-email")]
		[AllowAnonymous]
		public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
		{
			var result = await Sender.Send(new ConfirmEmailCommand(userId, token));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		//http://localhost:5000/api/auth/generate-email-confirmation-token?userId=123
		[HttpGet("generate-email-confirmation-token")]
		[AllowAnonymous]
		public async Task<IActionResult> GenerateEmailConfirmationToken([FromQuery] string userId)
		{
			var result = await Sender.Send(new GenerateEmailConfirmationTokenCommand(userId));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		//http://localhost:5000/api/auth/forgot-password
		[HttpPost("forgot-password")]
		[AllowAnonymous]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
		{
			var result = await Sender.Send(command);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		//http://localhost:5000/api/auth/reset-password
		[HttpPost("reset-password")]
		[AllowAnonymous]
		public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
		{
			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		//http://localhost:5000/api/auth/change-password
		[HttpPost("change-password")]
		[Authorize]
		public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var result = await Sender.Send(command with { UserId = userId! });


			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}



		// POST api/User/login
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginCommand command)
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



////| Method |          Route              |    Description       |
////| ------ | --------------------------- | -------------------- |
////| POST   | `/ api / auth / register`   | Register new user    |
////| POST   | `/ api / auth / login`      | Login and get token  |
////| POST   | `/api/auth/refresh-token`   | Refresh JWT token    |
////| POST   | `/api/auth/logout`          | Revoke refresh token |
////| POST   | `/api/auth/forgot-password` | Send reset link      |
////| POST   | `/api/auth/reset-password`  | Reset password       |
////| POST   | `/api/auth/verify-email`    | Verify email         |
