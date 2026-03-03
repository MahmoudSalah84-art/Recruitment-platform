using Jobs.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Jobs.Infrastructure.Identity
{
	public static class RoleSeeder
	{
		public static async Task SeedAsync( RoleManager<IdentityRole> roleManager)
		{
			var adminRole = new IdentityRole("Admin");

			if (!await roleManager.RoleExistsAsync("Admin"))
			{
				await roleManager.CreateAsync(adminRole);

				await roleManager.AddClaimAsync(adminRole,
					new Claim("Permission", Permissions.Jobs.Create));

				await roleManager.AddClaimAsync(adminRole,
					new Claim("Permission", Permissions.Jobs.Delete));
			}
		}
	}
}
