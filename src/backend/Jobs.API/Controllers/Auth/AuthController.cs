using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Identity.Command.ChangePassword;
using Jobs.Application.Features.Identity.Command.ConfirmEmail;
using Jobs.Application.Features.Identity.Command.ForgotPassword;
using Jobs.Application.Features.Identity.Command.GenerateEmailConfirmationToken;
using Jobs.Application.Features.Identity.Command.Login;
using Jobs.Application.Features.Identity.Command.RefreshToken;
using Jobs.Application.Features.Identity.Command.ResetPassword;
using Jobs.Application.Features.Identity.Command.RevokeToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Auth
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ApiController
	{
		/// <summary>
		/// Refreshes the authentication tokens using a valid refresh token.
		/// </summary>
		/// <param name="command">The refresh token request containing the refresh token.</param>
		/// <returns>
		/// Returns a new access token (and optionally a new refresh token).
		/// </returns>
		/// <remarks>
		/// - This endpoint is publicly accessible (AllowAnonymous).
		/// - Uses CQRS pattern via RefreshTokenCommand.
		/// - Typically used when the access token has expired.
		/// </remarks>
		/// <response code="200">Tokens refreshed successfully.</response>
		/// <response code="400">Invalid or expired refresh token.</response>
		/// <response code="401">Unauthorized - refresh token is missing or invalid.</response>
		// http://localhost:5000/api/auth/refresh-token
		[HttpPost("refresh-token")]
		[AllowAnonymous]
		public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
		{
			var result = await Sender.Send(command);
		
			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}


		/// <summary>
		/// Logs out the currently authenticated user by clearing authentication cookies.
		/// </summary>
		/// <returns>
		/// Returns a success response after clearing authentication tokens.
		/// </returns>
		/// <remarks>
		/// - Clears access token and refresh token cookies.
		/// - Works with cookie-based JWT authentication.
		/// - After logout, user must re-authenticate to access secured endpoints.
		/// </remarks>
		/// <response code="200">Logged out successfully.</response>
		/// <response code="401">User is not authenticated.</response>
		[HttpPost("logout")]
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

			var result = await Sender.Send(new RevokeTokenCommand(userId));

			var response = result.ToApiResponse<object>();

			// Clear authentication cookies
			Response.Cookies.Delete("accessToken");
			Response.Cookies.Delete("refreshToken");

			return StatusCode(response.StatusCode, response);
		}





		/// <summary>
		/// Confirms a user's email address using a verification token.
		/// </summary>
		/// <param name="token">The email confirmation token sent to the user.</param>
		/// <returns>
		/// Returns the result of the email confirmation operation.
		/// </returns>
		/// <remarks>
		/// - This endpoint is publicly accessible (AllowAnonymous).
		/// - The confirmation is performed using CQRS via ConfirmEmailCommand.
		/// - Typically invoked via a link sent to the user's email.
		/// </remarks>
		/// <response code="200">Email confirmed successfully.</response>
		/// <response code="400">Invalid or expired confirmation token.</response>
		/// <response code="404">User not found.</response>
		// http://localhost:5000/api/auth/confirm-email?userId=123&token=abc
		[HttpGet("confirm-email")]
		[AllowAnonymous]
		public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
		{
			//string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

			var result = await Sender.Send(new ConfirmEmailCommand(userId, token));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Generates an email confirmation token for a specific user.
		/// </summary>
		/// <param name="userId">The unique identifier of the user.</param>
		/// <returns>
		/// Returns a generated email confirmation token.
		/// </returns>
		/// <remarks>
		/// - This endpoint is publicly accessible (AllowAnonymous).
		/// - Uses CQRS pattern via GenerateEmailConfirmationTokenCommand.
		/// - The token is typically sent to the user's email for verification.
		/// </remarks>
		/// <response code="200">Token generated successfully.</response>
		/// <response code="400">Invalid user ID supplied.</response>
		/// <response code="404">User not found.</response>
		//http://localhost:5000/api/auth/generate-email-confirmation-token?userId=123
		[HttpPost("generate-email-confirmation-token")]
		[Authorize]
		public async Task<IActionResult> GenerateEmailConfirmationToken( GenerateEmailConfirmationTokenCommand command)
		{
			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Initiates the password reset process for a user.
		/// </summary>
		/// <param name="command">The forgot password request containing the user's email.</param>
		/// <returns>
		/// Returns a response indicating whether the password reset process was initiated successfully.
		/// </returns>
		/// <remarks>
		/// - This endpoint is publicly accessible (AllowAnonymous).
		/// - Uses CQRS pattern via ForgotPasswordCommand.
		/// - If the email exists, a password reset token is generated and sent to the user's email.
		/// - For security reasons, the response should not reveal whether the email exists or not.
		/// </remarks>
		/// <response code="200">Request processed successfully (check email if it exists).</response>
		/// <response code="400">Invalid request data.</response>
		//http://localhost:5000/api/auth/forgot-password
		[HttpPost("forgot-password")]
		[AllowAnonymous]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
		{
			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Resets the user's password using a valid reset token.
		/// </summary>
		/// <param name="command">The reset password request containing user email, reset token, and new password.</param>
		/// <returns>
		/// Returns the result of the password reset operation.
		/// </returns>
		/// <remarks>
		/// - This endpoint is publicly accessible (AllowAnonymous).
		/// - Uses CQRS pattern via ResetPasswordCommand.
		/// - The reset token must be valid, unexpired, and unused.
		/// - After successful reset, the token becomes invalid.
		/// </remarks>
		/// <response code="200">Password reset successfully.</response>
		/// <response code="400">Invalid request or expired token.</response>
		/// <response code="404">User not found.</response>
		//http://localhost:5000/api/auth/reset-password
		[HttpPost("reset-password")]
		[AllowAnonymous]
		public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
		{
			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Changes the password for the currently authenticated user.
		/// </summary>
		/// <param name="currentPassword">The user's current password.</param>
		/// <param name="newPassword">The new password to be set.</param>
		/// <returns>
		/// Returns the result of the password change operation.
		/// </returns>
		/// <remarks>
		/// - This endpoint requires authentication.
		/// - Uses CQRS pattern via ChangePasswordCommand.
		/// - The current password must be correct before allowing the change.
		/// </remarks>
		/// <response code="200">Password changed successfully.</response>
		/// <response code="400">Invalid input or new password does not meet requirements.</response>
		/// <response code="401">Unauthorized or incorrect current password.</response>
		//http://localhost:5000/api/auth/change-password
		[HttpPost("change-password")]
		[Authorize]
		public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
		{
			string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

			var result = await Sender.Send(new ChangePasswordCommand(userId, currentPassword, newPassword));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}


		/// <summary>
		/// Authenticates a user and issues access and refresh tokens.
		/// </summary>
		/// <param name="command">The login credentials (email/username and password).</param>
		/// <returns>
		/// Returns authentication tokens and user information upon successful login.
		/// </returns>
		/// <remarks>
		/// - This endpoint is publicly accessible.
		/// - Uses CQRS pattern via LoginCommand.
		/// - On successful authentication, access and refresh tokens are stored in HttpOnly cookies.
		/// - Access token is short-lived, while refresh token is used to renew sessions.
		/// </remarks>
		/// <response code="200">Login successful.</response>
		/// <response code="400">Invalid credentials or request data.</response>
		/// <response code="401">Unauthorized - incorrect email or password.</response>
		// POST http://localhost:5077/api/auth/login
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
