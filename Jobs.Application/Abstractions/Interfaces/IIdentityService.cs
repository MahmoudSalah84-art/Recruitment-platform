using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Abstractions.Interfaces
{

	public interface IIdentityService
	{
		Task<AuthResult> RegisterAsync(RegisterRequest request);
		Task<AuthResult> LoginAsync(LoginRequest request);
		Task<string> GenerateEmailConfirmationTokenAsync(string userId);
		Task<Result> ConfirmEmailAsync(string userId, string token);
		Task<string> GeneratePasswordResetTokenAsync(string email);
		Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
		Task<Result> AddToRoleAsync(string userId, string role);



		//Task<bool> CheckPasswordAsync(string userId, string password);
		//Task<bool> IsInRoleAsync(string userId, string role);
		//Task<bool> AuthorizeAsync(string userId, string policyName);
		//Task<string?> GetUserIdAsync(string userName);
	}
}