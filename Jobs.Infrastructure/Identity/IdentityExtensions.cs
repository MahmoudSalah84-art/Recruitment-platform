using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Jobs.Infrastructure.Identity
{
	public static class IdentityExtensions
	{
		public static IServiceCollection AddJobSiteIdentity(this IServiceCollection services, string identityConnectionString)
		{
			services.AddDbContext<IdentityDbContext>(options =>
				options.UseSqlServer(identityConnectionString) );

			services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
			{
				// password settings
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;

				// lockout settings
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;

				// user settings
				options.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<IdentityDbContext>()
			.AddDefaultTokenProviders();

			return services;
		}
	}
}
