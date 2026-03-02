using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Common.Interfaces;
using Jobs.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Jobs.Infrastructure.Services
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roles;
		private readonly IJwtTokenService _jwtTokenService;
		private readonly AppIdentityDbContext _context;

		public IdentityService(
		UserManager<AppUser> userManager,
		RoleManager<AppRole> roles,
		IJwtTokenService jwtTokenService,
		AppIdentityDbContext dbContext	)
		{
			_userManager = userManager;
			_jwtTokenService = jwtTokenService;
			_roles = roles;
			_context = dbContext;
		}


		// ── Register ─────────────────────────────────────────────────────────────
		public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest req)
		{
			if (await _userManager.FindByEmailAsync(req.Email) != null)
				return Result<AuthResponse>.Failure("Email already registered.");

			var user = new AppUser
			{
				FirstName = req.FirstName,
				LastName = req.LastName,
				Email = req.Email,
				UserName = req.Email
			};

			var result = await _userManager.CreateAsync(user, req.Password);
			if (!result.Succeeded) return Result<AuthResponse>.Failure(result.Errors.Select(e => e.Description).ToString()!);

			if (!await _roles.RoleExistsAsync("User"))
				await _roles.CreateAsync(new AppRole { Name = "User" });

			await _userManager.AddToRoleAsync(user, "User");

			return await BuildAuthAsync(user);
		}
		// ── Login ─────────────────────────────────────────────────────────────────
		public async Task<Result<AuthResponse>> LoginAsync(LoginRequest req)
		{
			var user = await _userManager.FindByEmailAsync(req.Email);
			if (user == null || !user.IsActive) return 
					Result<AuthResponse>.Failure("Invalid credentials.");

			if (!await _userManager.CheckPasswordAsync(user, req.Password)) 
				return Result<AuthResponse>.Failure("Invalid credentials.");

			return await BuildAuthAsync(user);
		}
		// ── Refresh Token ─────────────────────────────────────────────────────────
		public async Task<Result<AuthResponse>> RefreshTokenAsync(string accessToken, string refreshToken)
		{
			var principal = _jwtTokenService.GetPrincipalFromExpiredToken(accessToken);
			if (principal == null) return Result<AuthResponse>.Failure("Invalid access token.");

			var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

			var user = await _userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == userId);
			if (user == null) return Result<AuthResponse>.Failure("User not found.");

			var hashedToken = HashToken(refreshToken);

			var stored = user.RefreshTokens.FirstOrDefault(t => t.TokenHash == hashedToken);
			if (stored == null || !stored.IsActive) 
				return Result<AuthResponse>.Failure("Invalid or expired refresh token.");

			stored.IsUsed = true;

			_context.RefreshTokens.Update(stored);
			await _context.SaveChangesAsync();

			return await BuildAuthAsync(user);
		}

		// ── Revoke Token ──────────────────────────────────────────────────────────
		public async Task<Result> RevokeRefreshTokenAsync(string refreshToken)
		{
			var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == refreshToken);
			if (token == null) return Result.Failure("Token not found.");
			if (!token.IsActive) return Result.Failure("Token already inactive.");

			token.IsRevoked = true;

			_context.RefreshTokens.Update(token);
			await _context.SaveChangesAsync();
			return Result.Success();
		}



		// ── Roles ─────────────────────────────────────────────────────────────────
		public async Task<Result> CreateRoleAsync(string Name, string Description)
		{
			if (await _roles.RoleExistsAsync(Name)) return Result.Failure("Role already exists.");

			var res = await _roles.CreateAsync(new AppRole { Name = Name, Description = Description });

			return res.Succeeded ? Result.Success() : Result.Failure(res.Errors.Select(e => e.Description).ToString()!);
		}

		public async Task<Result> DeleteRoleAsync(string roleName)
		{
			var role = await _roles.FindByNameAsync(roleName);
			if (role == null) return Result.Failure("Role not found.");

			var res = await _roles.DeleteAsync(role);
			return res.Succeeded ? Result.Success() : Result.Failure(res.Errors.Select(e => e.Description).ToString()!);
		}

		public async Task<Result> AssignRoleAsync(string UserId, string RoleName)
		{
			var user = await _userManager.FindByIdAsync(UserId);
			if (user == null) return Result.Failure("User not found.");

			if (!await _roles.RoleExistsAsync(RoleName)) return Result.Failure("Role not found.");

			var res = await _userManager.AddToRoleAsync(user, RoleName);

			return res.Succeeded ? Result.Success() : Result.Failure(res.Errors.Select(e => e.Description).ToString()!);
		}

		public async Task<Result> RemoveRoleAsync(string UserId, string RoleName)
		{
			var user = await _userManager.FindByIdAsync(UserId);
			if (user == null) return Result.Failure("User not found.");

			var res = await _userManager.RemoveFromRoleAsync(user, RoleName);

			return res.Succeeded ? Result.Success() : Result.Failure(res.Errors.Select(e => e.Description).ToString()!);
		}

		public async Task<Result<IEnumerable<string>>> GetAllRolesAsync() =>
			Result<IEnumerable<string>>.Success(await _roles.Roles.Select(r => r.Name!).ToListAsync());

		
		// ── Permissions ───────────────────────────────────────────────────────────
		public async Task<Result> AssignPermissionToRoleAsync(string RoleName, string Permission)
		{
			var role = await _roles.FindByNameAsync(RoleName);
			if (role == null) return Result.Failure("Role not found.");

			var claims = await _roles.GetClaimsAsync(role);
			if (claims.Any(c => c.Type == "Permission" && c.Value == Permission))
				return Result.Failure("Permission already assigned.");

			var res = await _roles.AddClaimAsync(role, new Claim("Permission", Permission));

			return res.Succeeded ? Result.Success() : Result.Failure(res.Errors.Select(e => e.Description).ToString()!);
		}

		public async Task<Result> RemovePermissionFromRoleAsync(string RoleName, string Permission)
		{
			var role = await _roles.FindByNameAsync(RoleName);
			if (role == null) return Result.Failure("Role not found.");

			var res = await _roles.RemoveClaimAsync(role, new Claim("Permission",Permission));
			return res.Succeeded ? Result.Success() : Result.Failure(res.Errors.Select(e => e.Description).ToString()!);
		}

		public async Task<Result<IEnumerable<string>>> GetRolePermissionsAsync(string roleName)
		{
			var role = await _roles.FindByNameAsync(roleName);
			if (role == null) return Result<IEnumerable<string>>.Failure("Role not found.");
			var claims = await _roles.GetClaimsAsync(role);
			return Result<IEnumerable<string>>.Success(claims.Where(c => c.Type == "Permission").Select(c => c.Value));
		}

		public async Task<Result<IEnumerable<string>>> GetUserPermissionsAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) return Result<IEnumerable<string>>.Failure("User not found.");

			var roleNames = await _userManager.GetRolesAsync(user);

			var perms = new HashSet<string>();
			foreach (var rn in roleNames)
			{
				var role = await _roles.FindByNameAsync(rn);
				if (role == null) continue;

				var claims = (await _roles.GetClaimsAsync(role)).Where(c => c.Type == "Permission");
				foreach (var c in claims)
					perms.Add(c.Value);
			}
			return Result<IEnumerable<string>>.Success(perms);
		}



		// ── Users ─────────────────────────────────────────────────────────────────
		public async Task<Result<IEnumerable<UserDto>>> GetUsersAsync()
		{
			var list = await _userManager.Users.ToListAsync();
			var dtos = new List<UserDto>();
			foreach (var u in list)
			{
				var roles = await _userManager.GetRolesAsync(u);
				dtos.Add(new UserDto
				{
					Id = u.Id,
					Email = u.Email!,
					UserName = u.UserName!,
					FirstName = u.FirstName,
					LastName = u.LastName,
					IsEmailVerified = u.EmailConfirmed,
					IsActive = u.IsActive,
					Roles = roles.ToList(),
				});
			}
			return Result<IEnumerable<UserDto>>.Success(dtos);
		}

		public async Task<Result<UserDto>> GetUserByIdAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) return Result<UserDto>.Failure("User not found.");

			var roles = await _userManager.GetRolesAsync(user);

			return Result<UserDto>.Success(new UserDto
			{
				Id = user.Id,
				Email = user.Email!,
				UserName = user.UserName!,
				FirstName = user.FirstName,
				LastName = user.LastName,
				IsEmailVerified = user.EmailConfirmed,
				IsActive = user.IsActive,
				Roles = roles.ToList(),
			});
		}

		public async Task<Result> ToggleUserActiveAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) return Result.Failure("User not found.");

			user.IsActive = !user.IsActive;

			await _userManager.UpdateAsync(user);

			return Result<string>.Success(user.IsActive ? "User activated." : "User deactivated.");
		}

		//_______ Email Confirmation ─────────────────────────────────────────────────
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


		//_______ Password Reset ───────────────────────────────────────────────────────────
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

			var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

			return result.Succeeded
				? Result.Success()
				: Result.Failure(result.Errors.Select(e => e.Description).ToString()!);
		}
		




		// ── Helper ────────────────────────────────────────────────────────────────
		private string HashToken(string token)
		{
			using var sha256 = SHA256.Create();
			var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
			return Convert.ToBase64String(bytes);
		}






		private async Task<Result<AuthResponse>> BuildAuthAsync(AppUser user)
		{
			var roleNames = await _userManager.GetRolesAsync(user);

			var perms = new HashSet<string>();
			foreach (var rn in roleNames)
			{
				var role = await _roles.FindByNameAsync(rn);
				if (role == null) continue;
				var claims = await _roles.GetClaimsAsync(role);
				foreach (var c in claims.Where(c => c.Type == "Permission")) perms.Add(c.Value);
			}

			var accessToken = _jwtTokenService.GenerateAccessToken(user.Id,user.Email!,user.FirstName,user.LastName, roleNames, perms);
			var refreshToken = _jwtTokenService.GenerateRefreshToken();

			await _context.RefreshTokens.AddAsync(new RefreshToken
			{
				TokenHash = refreshToken,
				UserId = user.Id,
				ExpiryDate = DateTime.UtcNow.AddDays(7)
			});

			await _context.SaveChangesAsync();

			return Result<AuthResponse>.Success(new AuthResponse(
				accessToken, refreshToken, DateTime.UtcNow.AddMinutes(60),
				user.Id, user.Email!, roleNames, perms));
		}
	}
}