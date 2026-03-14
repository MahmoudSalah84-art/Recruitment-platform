using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Common.Interfaces;
using Jobs.Domain.IRepositories;
using Jobs.Infrastructure.BackgroundJobs;
using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Data.Interceptors;
using Jobs.Infrastructure.Identity;
using Jobs.Infrastructure.Repositories.Repo;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Jobs.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jobs.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			// EF Core context for domain data
			services.AddDbContext<JobDbContext>((sp, options) =>
			{
				// 1. جلب النسخ من الـ Service Provider (sp)
				//var outboxInterceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
				//var softDeleteInterceptor = sp.GetRequiredService<SoftDeleteInterceptor>();

				options.UseSqlServer(
					configuration.GetConnectionString("DefaultConnection"),
					sql => sql.EnableRetryOnFailure() );
				//.AddInterceptors(softDeleteInterceptor, outboxInterceptor);
			});

			// Interceptors
			services.AddScoped<SaveChangesInterceptor, SoftDeleteInterceptor>();
			services.AddSingleton<SaveChangesInterceptor, ConvertDomainEventsToOutboxMessagesInterceptor>();


			// Identity DB (separate)
			services.AddJobSiteIdentity(configuration.GetConnectionString("IdentityConnection")!);

			// Repositories
			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICompanyRepository, CompanyRepository>();
			services.AddScoped<IJobRepository,JobRepository>();
			services.AddScoped<IApplicationRepository, ApplicationRepository>();
			services.AddScoped<ICVRepository, CVRepository>();

			// Unit of Work
			services.AddScoped<IUnitOfWork, UnitOfWork>();


			// Services
			services.AddSingleton<IFileStorageService>(_ => new LocalFileStorageService("files"));
			//var emailOptions = new EmailServiceOptions();
			//configureEmail?.Invoke(emailOptions);
			//services.AddSingleton(emailOptions);
			//services.AddScoped<IEmailService, EmailService>();

			//JWT generator options placeholder
			services.Configure<JwtSettings>(
						configuration.GetSection("JwtSettings"));

			services.AddScoped<IJwtTokenService, JwtTokenService>();
			services.AddScoped<ICurrentUserService, CurrentUserService>();
			services.AddScoped<IIdentityService, IdentityService>();

			// Background Service
			services.AddHostedService<OutboxProcessor>();

			return services;
		}
	}
}
