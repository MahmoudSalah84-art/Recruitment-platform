using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Abstractions.Interfaces
{

	public interface IIdentityService
	{
		// ── Register & Login ──────────────────────────────────────────────────────
		Task<Result<AuthResponse>> RegisterAsync(RegisterRequest req);
		Task<Result<AuthResponse>> LoginAsync(LoginRequest req);

		// ── Tokens ────────────────────────────────────────────────────────────────
		Task<Result<AuthResponse>> RefreshTokenAsync(string accessToken, string refreshToken);
		Task<Result> RevokeRefreshTokenAsync(string refreshToken);

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
		Task<Result<UserDto>> GetUserByIdAsync(string userId);
		Task<Result> ToggleUserActiveAsync(string userId);

		// ── Email Confirmation ────────────────────────────────────────────────────
		Task<string> GenerateEmailConfirmationTokenAsync(string userId);
		Task<Result> ConfirmEmailAsync(string userId, string token);

		// ── Password Reset ────────────────────────────────────────────────────────
		Task<string> GeneratePasswordResetTokenAsync(string email);
		Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
	}
}