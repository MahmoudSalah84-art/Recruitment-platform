using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Jobs.Infrastructure.Identity
{
	public static class IdentityExtensions
	{
		public static IServiceCollection AddJobSiteIdentity(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AppIdentityDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"), sql => { sql.EnableRetryOnFailure(); }) 
			);

			services.AddIdentity<AppUser, AppRole>(options =>
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
			.AddEntityFrameworkStores<AppIdentityDbContext>()
			.AddDefaultTokenProviders();




			services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options => {
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,

				ValidIssuer = configuration["JwtSettings:Issuer"],
				ValidAudience = configuration["JwtSettings:Audience"],
				ClockSkew = TimeSpan.Zero,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!))
			};

			options.Events = new JwtBearerEvents
			{
				OnMessageReceived = context =>
				{

					var accessToken = context.Request.Cookies["accessToken"];
					if (!string.IsNullOrEmpty(accessToken))
					{
						context.Token = accessToken;
					}
					return Task.CompletedTask;
				}
			};
});


			return services;
		}
	}
}
