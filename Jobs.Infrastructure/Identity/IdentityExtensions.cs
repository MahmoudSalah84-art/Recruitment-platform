using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Identity
{
	public static class IdentityExtensions
	{
		public static IServiceCollection AddJobSiteIdentity(this IServiceCollection services, string identityConnectionString)
		{
			services.AddDbContext<IdentityDbContext>(options =>
			{
				options.UseSqlServer(identityConnectionString);
			});

			services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
			{
				options.User.RequireUniqueEmail = true; 
				options.Password.RequireNonAlphanumeric = false;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);// defult 5 minutes
			})
				.AddEntityFrameworkStores<IdentityDbContext>()
				.AddDefaultTokenProviders();

			return services;
		}
	}
}
