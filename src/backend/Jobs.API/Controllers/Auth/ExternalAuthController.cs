using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation;
using Jobs.Application.Features.Identity.Command.GoogleLogin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Auth
{
	public class ExternalAuthController : ApiController
	{
		/// <summary>
		/// Authenticates a user using a Google ID Token.
		/// </summary>
		/// <remarks>
		/// Flow:
		/// 1. Receives an ID Token from the client (generated after Google Sign-In).
		/// 2. Sends the token to the application layer (GoogleLoginCommand) to validate it with Google.
		/// 3. If the token is valid:
		///    - Authenticates or retrieves the user from the system.
		///    - Generates Access Token and Refresh Token.
		/// 4. Stores the tokens in HttpOnly cookies for secure usage.
		///
		/// Cookies:
		/// - accessToken:
		///   Used to authenticate API requests. Short-lived.
		/// - refreshToken:
		///   Used to obtain new access tokens. Longer-lived (7 days).
		///
		/// Security Notes:
		/// - HttpOnly: Prevents access via JavaScript (protects against XSS).
		/// - SameSite=Strict: Prevents sending cookies in cross-site requests.
		/// - Secure=false: Should be set to true in production (HTTPS only).
		/// </remarks>
		/// <param name="request">
		/// Contains the Google ID Token received from the frontend.
		/// </param>
		/// <returns>
		/// Returns the result of the operation (success or failure) with token data if successful.
		/// </returns>
		[HttpPost("google-login")]
		[AllowAnonymous]
		public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
		{
			var result = await Sender.Send(new GoogleLoginCommand(request.IdToken));

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

		public sealed record GoogleLoginRequest(string IdToken);




		//CreateCVJobRecommendationCommand_cv
		//CreateCVJobRecommendationCommand_job

		[HttpPost("testingforJob")]
		[AllowAnonymous]
		public async Task<IActionResult> testingforJob([FromBody] string JobIdThatCreated)
		{
			var result = await Sender.Send(new CreateCVJobRecommendationCommand_job(JobIdThatCreated));

			var response = result.ToApiResponse<object>();
			return StatusCode(response.StatusCode, response);
		}


		[HttpPost("testingforCv")]
		[AllowAnonymous]
		public async Task<IActionResult> testingforCv([FromQuery] string userIdThatUplodeCv)
		{
			var result = await Sender.Send(new CreateCVJobRecommendationCommand_cv(userIdThatUplodeCv));

			var response = result.ToApiResponse<object>();
			return StatusCode(response.StatusCode, response);
		}
	}


}


	

//| Method | Route                      | Description        |
//| ------ | --------------------       | ------------------- |
//| POST   | `/ api / auth / google`    | Login with Google   |
//| POST   | `/api/auth/facebook`       | Login with Facebook |
//| POST   | `/api/auth/linkedin`       | Login with LinkedIn |
