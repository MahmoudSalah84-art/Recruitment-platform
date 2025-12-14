using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Identity;
using Jobs.Infrastructure.Outbox;
using Jobs.Infrastructure.Repositories.Repo;
using Jobs.Infrastructure.Rules;
using Jobs.Infrastructure.Services;
using Jobs.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, string jobDbConnectionString, string identityDbConnectionString, Action<EmailServiceOptions>? configureEmail = null)
		{
			// EF Core context for domain data
			services.AddDbContext<JobDbContext>(options =>
			{
				options.UseSqlServer(jobDbConnectionString, sql =>
				{
					sql.EnableRetryOnFailure();
				});
			});

			// Identity DB (separate)
			services.AddDbContext<IdentityDbContext>(options =>
			{
				options.UseSqlServer(identityDbConnectionString, sql =>
				{
					sql.EnableRetryOnFailure();
				});
			});

			// Identity
			services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole<Guid>>()
				.AddEntityFrameworkStores<IdentityDbContext>()
				.AddDefaultTokenProviders();

			// Repositories
			services.AddScoped(typeof(BaseRepository<>));
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICompanyRepository, CompanyRepository>();
			services.AddScoped<IJobRepository, JobRepository>();
			services.AddScoped<IApplicationRepository, ApplicationRepository>();
			services.AddScoped<ICVRepository, CVRepository>();

			// Unit of Work
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			// Outbox
			services.AddScoped<IOutboxPublisher, OutboxPublisher>();

			// Rules
			services.AddScoped<IUniqueEmailRule, UniqueEmailRule>();

			// Services
			services.AddSingleton<IFileStorageService>(_ => new LocalFileStorageService("files"));
			var emailOptions = new EmailServiceOptions();
			configureEmail?.Invoke(emailOptions);
			services.AddSingleton(emailOptions);
			services.AddScoped<IEmailService, EmailService>();

			// JWT generator options placeholder
			services.Configure<JwtOptions>(opts =>
			{
				opts.Issuer = "JobSite";
				opts.Audience = "JobSiteClients";
				opts.SecretKey = "replace-this-with-secure-long-secret";
				opts.ExpiryMinutes = 60;
			});
			services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

			// Interceptors
			services.AddScoped<SaveChangesInterceptor, SoftDeleteInterceptor>();

			return services;
		}
	}
}
