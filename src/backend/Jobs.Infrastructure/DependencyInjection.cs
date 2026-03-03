using Jobs.Application.Abstractions.Interfaces;
using Jobs.Domain.Repositories.UnitOfWork;
using Jobs.Domain.Repository.Repo;
using Jobs.Infrastructure.BackgroundJobs;
using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Data.Interceptors;
using Jobs.Infrastructure.Identity;
using Jobs.Infrastructure.Repositories.Repo;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Jobs.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Jobs.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, string jobDbConnectionString, string identityDbConnectionString, Action<EmailServiceOptions>? configureEmail = null)
		{
			// EF Core context for domain data
			services.AddDbContext<JobDbContext>((sp, options) =>
			{
				// 1. جلب النسخ من الـ Service Provider (sp)
				//var outboxInterceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
				//var softDeleteInterceptor = sp.GetRequiredService<SoftDeleteInterceptor>();

				options.UseSqlServer( jobDbConnectionString, sql => sql.EnableRetryOnFailure() );
				//.AddInterceptors(softDeleteInterceptor, outboxInterceptor);
			});

			// Identity DB (separate)
			services.AddDbContext<IdentityDbContext>(options =>
			{
				options.UseSqlServer(identityDbConnectionString, sql =>
				{
					sql.EnableRetryOnFailure();
				});
			});

			

			// Repositories
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICompanyRepository, CompanyRepository>();
			services.AddScoped<IJobRepository,JobRepository>();
			services.AddScoped<IApplicationRepository, ApplicationRepository>();
			services.AddScoped<ICVRepository, CVRepository>();

			// Unit of Work
			services.AddScoped<IUnitOfWork, UnitOfWork>();


			// Services
			services.AddSingleton<IFileStorageService>(_ => new LocalFileStorageService("files"));
			var emailOptions = new EmailServiceOptions();
			configureEmail?.Invoke(emailOptions);
			services.AddSingleton(emailOptions);
			services.AddScoped<IEmailService, EmailService>();

			// JWT generator options placeholder
			services.Configure<JwtSettings>(
						configuration.GetSection("JwtSettings"));

			services.AddScoped<IJwtTokenService, JwtTokenService>();

			// Interceptors
			services.AddScoped<SaveChangesInterceptor, SoftDeleteInterceptor>();
			services.AddSingleton<SaveChangesInterceptor , ConvertDomainEventsToOutboxMessagesInterceptor>();


			// Background Service
			services.AddHostedService<OutboxProcessor>();


			return services;
		}
	}
}
