using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Abstractions.Interfaces
{

	public interface IIdentityService
	{
		// ── Register & Login ──────────────────────────────────────────────────────
		Task<Result<AuthResponse>> RegisterAsync(RegisterRequest req);
		Task<Result<AuthResponse>> LoginAsync(string Email, string Password);
		Task<Result<AuthResponse>> LoginWithGoogleAsync(string idToken);

		// ── Tokens ────────────────────────────────────────────────────────────────
		Task<Result<AuthResponse>> RefreshTokenAsync(string accessToken, string refreshToken);
		Task<Result> RevokeRefreshTokenAsync(string UserId);

		// ── Roles ─────────────────────────────────────────────────────────────────
		Task<Result> CreateRoleAsync(string Name, string Description);
		Task<Result> DeleteRoleAsync(string roleName);
		Task<Result> AssignRoleAsync(string UserId, string RoleName);
		Task<Result> RemoveRoleAsync(string UserId, string RoleName);
		Task<Result<IEnumerable<string>>> GetAllRolesAsync();

		// ── Permissions ───────────────────────────────────────────────────────────
		Task<Result> AssignPermissionToRoleAsync(string RoleName, string Permission);
		Task<Result> RemovePermissionFromRoleAsync(string RoleName, string Permission);
		Task<Result<IEnumerable<string>>> GetRolePermissionsAsync(string roleName);
		Task<Result<IEnumerable<string>>> GetUserPermissionsAsync(string userId);

		// ── Users ─────────────────────────────────────────────────────────────────
		Task<Result<IEnumerable<UserDto>>> GetUsersAsync();
		Task<Result<UserDto>> GetUserByEmailAsync(string email);
		Task<Result<UserDto>> GetUserByIdAsync(string userId);
		Task<Result> ToggleUserActiveAsync(string userId);
		Task<Result> DeleteUserAsync(string userId);

		// ── Email Confirmation ────────────────────────────────────────────────────
		Task<Result<string>> GenerateEmailConfirmationTokenAsync(string userId);
		Task<Result> ConfirmEmailAsync(string userId, string token);

		//Task<Result> ResendConfirmationEmailAsync(string email);

		// ── Password Reset ────────────────────────────────────────────────────────
		Task<string?> GeneratePasswordResetTokenAsync(string email);
		Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
		Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

	}
}


//Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request);


//Task<Result<TwoFactorSetupResponse>> Enable2FAAsync(string userId);
//Task<Result> Verify2FAAsync(string userId, Enable2FARequest request);
//Task<Result> Disable2FAAsync(string userId);
//Task<Result<AuthResponse>> ExternalLoginAsync(ExternalLoginRequest request);
