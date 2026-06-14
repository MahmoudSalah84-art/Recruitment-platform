using Jobs.Application.Common.Helpers;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Jobs.Infrastructure.Identity.Seeder
{
	public static class IdentitySeeder
	{
		public static async Task SeedAsync( RoleManager<AppRole> roleManager)
		{
			await SeedRoles(roleManager);
			await SeedPermissions(roleManager);
		}

		




		private static async Task SeedRoles(RoleManager<AppRole> roleManager)
		{
			var allroles = RolesHelper.GetAllRoles();

			foreach (var role in allroles)
			{
				if (!await roleManager.RoleExistsAsync(role!))
				{
					await roleManager.CreateAsync(new AppRole(role!));
				}
			}
		}

		private static async Task SeedPermissions(RoleManager<AppRole> roleManager)
		{
			var allPermissions = PermissionsHelper.GetAllPermissions();

			var adminRole = await roleManager.FindByNameAsync(Roles.Admin) ?? throw new ArgumentException("there is not this role yet");




			// admin
			foreach (var permission in allPermissions)
			{
				await AddPermissionIfNotExists(roleManager, adminRole, permission!);
			}




			//  Company
			var companyRole = await roleManager.FindByNameAsync(Roles.Company) ?? throw new ArgumentException("there is not this role yet"); ;

			var companyPermissions = allPermissions
				.Where(p => p!.StartsWith("Jobs") ||
				p == Permissions.Applications_View ||
				p.StartsWith("Companies"))
				.ToList();
			foreach (var permission in companyPermissions)
			{
				await AddPermissionIfNotExists(roleManager, companyRole, permission!);
			}






			// Seeker
			var userRole = await roleManager.FindByNameAsync(Roles.User) ?? throw new ArgumentException("there is not this role yet"); ;

			var userPermissions = allPermissions
				.Where(p =>
					p == Permissions.Jobs_View ||
					p!.StartsWith("Applications") ||
					p!.StartsWith("CVs") ||
					p.StartsWith("Users"))
				.ToList();

			foreach (var permission in userPermissions)
			{
				await AddPermissionIfNotExists(roleManager, userRole, permission!);
			}
		}


		// ---------helper method to add permission to role if it does not exist
		private static async Task AddPermissionIfNotExists( RoleManager<AppRole> roleManager, AppRole role, string permission)
		{
			var claims = await roleManager.GetClaimsAsync(role);

			if (!claims.Any(c => c.Type == "Permission" && c.Value == permission))
			{
				await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
			}
		}
	}
}