using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Common.Interfaces;
using Jobs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Jobs.Infrastructure.Services
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IJwtTokenService _jwtTokenService;

		public IdentityService(
		UserManager<AppUser> userManager,
		IJwtTokenService jwtTokenService)
		{
			_userManager = userManager;
			_jwtTokenService = jwtTokenService;
		}

		public async Task<AuthResult> RegisterAsync(RegisterRequest request)
		{
			var user = new AppUser
			{
				Email = request.Email,
				UserName = request.UserName ?? request.Email
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
			{
				return new AuthResult(
					IsSuccess: false,
					AccessToken: null,
					RefreshToken: null,
					Errors: result.Errors.Select(e => e.Description)
				);
			}

			var roles = new List<string>();
			
			var accessToken = _jwtTokenService.GenerateAccessToken( user.Id, user.Email, roles, claims: null!);

			var refreshToken = _jwtTokenService.GenerateRefreshToken();

			return new AuthResult(
					IsSuccess: true,
					AccessToken: accessToken,
					RefreshToken: refreshToken,
					Errors: null
			);
		}

		public async Task<AuthResult> LoginAsync(LoginRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);

			if (user == null)
				return new AuthResult ( 
					IsSuccess : false,
					AccessToken: null,
					RefreshToken: null,
					Errors: new[] { "Invalid email or password." }
				);

			var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);

			if (!validPassword)
				return new AuthResult(
					IsSuccess: false,
					AccessToken: null,
					RefreshToken: null,
					Errors: new[] { "Invalid email or password." }
				);

			var roles = await _userManager.GetRolesAsync(user);

			var accessToken = _jwtTokenService.GenerateAccessToken(
				user.Id, user.Email!, roles, claims: null!);

			var refreshToken = _jwtTokenService.GenerateRefreshToken();

			return new AuthResult(
					IsSuccess: true,
					AccessToken: accessToken,
					RefreshToken: refreshToken,
					Errors: null
			);
		}

		public async Task<Result> AddToRoleAsync(string userId, string role)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
				return Result.Failure("User not found");

			var result = await _userManager.AddToRoleAsync(user, role);

			return result.Succeeded
				? Result.Success()
				: Result.Failure(result.Errors.Select(e => e.Description).ToString()!);
		}

		public async Task<bool> CheckPasswordAsync(string userId, string password)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) return false;

			return await _userManager.CheckPasswordAsync(user, password);
		}

		public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			return await _userManager.GenerateEmailConfirmationTokenAsync(user!);
		}

		public async Task<Result> ConfirmEmailAsync(string userId, string token)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
				return Result.Failure("User not found");

			var result = await _userManager.ConfirmEmailAsync(user, token);

			return result.Succeeded
				? Result.Success()
				: Result.Failure(result.Errors.Select(e => e.Description).ToString()!);
		}
		public async Task<string> GeneratePasswordResetTokenAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			return await _userManager.GeneratePasswordResetTokenAsync(user!);
		}

		public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);

			if (user == null)
				return Result.Failure("User not found");

			var result = await _userManager.ResetPasswordAsync( user, request.Token, request.NewPassword);

			return result.Succeeded
				? Result.Success()
				: Result.Failure(result.Errors.Select(e => e.Description).ToString()!);
		}

        
    }
}