using Jobs.Application.Features.Identity.Commands.ChangePassword;
using Jobs.Application.Features.Identity.Commands.ForgotPassword;
using Jobs.Application.Features.Identity.Commands.Login;
using Jobs.Application.Features.Identity.Commands.Logout;
using Jobs.Application.Features.Identity.Commands.RefreshToken;
using Jobs.Application.Features.Identity.Commands.Register;
using Jobs.Application.Features.Identity.Commands.ResendVerificationEmail;
using Jobs.Application.Features.Identity.Commands.ResetPassword;
using Jobs.Application.Features.Identity.Commands.VerifyEmail;
using Jobs.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using Serilog;
using System.Security.Claims;

namespace Jobs.API.Controllers.Auth
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// تسجيل مستخدم جديد
		/// </summary>
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			var command = new RegisterCommand
			{
				Email = dto.Email,
				UserName = dto.UserName,
				Password = dto.Password,
				ConfirmPassword = dto.ConfirmPassword,
				FirstName = dto.FirstName,
				LastName = dto.LastName
			};

			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message, errors = result.Errors });

			return Ok(new { message = result.Message, data = result.Data });
		}

		/// <summary>
		/// تسجيل دخول مستخدم
		/// </summary>
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var command = new LoginCommand
			{
				EmailOrUserName = dto.EmailOrUserName,
				Password = dto.Password,
				RememberMe = dto.RememberMe
			};

			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return Unauthorized(new { message = result.Message, errors = result.Errors });

			return Ok(new { message = result.Message, data = result.Data });
		}

		/// <summary>
		/// تحديث رمز الوصول باستخدام رمز التحديث
		/// </summary>
		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
		{
			var command = new RefreshTokenCommand
			{
				RefreshToken = dto.RefreshToken
			};

			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return Unauthorized(new { message = result.Message });

			return Ok(new { data = result.Data });
		}

		/// <summary>
		/// تسجيل خروج المستخدم
		/// </summary>
		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

			var command = new LogoutCommand { UserId = userId };
			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message });

			return Ok(new { message = result.Message });
		}

		/// <summary>
		/// التحقق من البريد الإلكتروني
		/// </summary>
		[HttpPost("verify-email")]
		public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto dto)
		{
			var command = new VerifyEmailCommand
			{
				Token = dto.Token,
				Email = dto.Email
			};

			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message });

			return Ok(new { message = result.Message });
		}

		/// <summary>
		/// إعادة إرسال بريد التحقق
		/// </summary>
		[HttpPost("resend-verification")]
		public async Task<IActionResult> ResendVerification([FromBody] ForgotPasswordDto dto)
		{
			var command = new ResendVerificationEmailCommand { Email = dto.Email };
			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message });

			return Ok(new { message = result.Message });
		}

		/// <summary>
		/// تغيير كلمة المرور
		/// </summary>
		[Authorize]
		[HttpPost("change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
		{
			var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

			var command = new ChangePasswordCommand
			{
				UserId = userId,
				CurrentPassword = dto.CurrentPassword,
				NewPassword = dto.NewPassword,
				ConfirmNewPassword = dto.ConfirmNewPassword
			};

			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message, errors = result.Errors });

			return Ok(new { message = result.Message });
		}

		/// <summary>
		/// نسيت كلمة المرور
		/// </summary>
		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
		{
			var command = new ForgotPasswordCommand { Email = dto.Email };
			var result = await _mediator.Send(command);

			return Ok(new { message = result.Message });
		}

		/// <summary>
		/// إعادة تعيين كلمة المرور
		/// </summary>
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
		{
			var command = new ResetPasswordCommand
			{
				Token = dto.Token,
				Email = dto.Email,
				NewPassword = dto.NewPassword,
				ConfirmNewPassword = dto.ConfirmNewPassword
			};

			var result = await _mediator.Send(command);

			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message, errors = result.Errors });

			return Ok(new { message = result.Message });
		}

		/// <summary>
		/// الحصول على معلومات المستخدم الحالي
		/// </summary>
		[Authorize]
		[HttpGet("me")]
		public async Task<IActionResult> GetCurrentUser()
		{
			var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

			var query = new GetUserByIdQuery { UserId = userId };
			var result = await _mediator.Send(query);

			if (!result.IsSuccess)
				return NotFound(new { message = result.Message });

			return Ok(new { data = result.Data });
		}
	}
}



//| Method |          Route              |    Description       |
//| ------ | --------------------------- | -------------------- |
//| POST   | `/ api / auth / register`   | Register new user    |
//| POST   | `/ api / auth / login`      | Login and get token  |
//| POST   | `/api/auth/refresh-token`   | Refresh JWT token    |
//| POST   | `/api/auth/logout`          | Revoke refresh token |
//| POST   | `/api/auth/forgot-password` | Send reset link      |
//| POST   | `/api/auth/reset-password`  | Reset password       |
//| POST   | `/api/auth/verify-email`    | Verify email         |
