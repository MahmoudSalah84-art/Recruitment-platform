using CloudinaryDotNet;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Jobs.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{


			
			// EF Core context for domain data
			services.AddDbContext<JobDbContext>((sp, options) =>
			{
				var outboxInterceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>(); //2
				var softDeleteInterceptor = sp.GetRequiredService<SoftDeleteInterceptor>(); //1
				var updateTimestampsInterceptor = sp.GetRequiredService<UpdateTimestampsInterceptor>();//3

				options.UseSqlServer(
					configuration.GetConnectionString("DefaultConnection"),
					sql => sql.EnableRetryOnFailure() )
				.AddInterceptors(softDeleteInterceptor, outboxInterceptor, updateTimestampsInterceptor);
			});



			// Interceptors
			services.AddScoped<SoftDeleteInterceptor>();
			services.AddScoped<ConvertDomainEventsToOutboxMessagesInterceptor>();
			services.AddScoped<UpdateTimestampsInterceptor>();



			// Identity DB (separate)
			services.AddJobSiteIdentity(configuration);

			// Repositories
			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICompanyRepository, CompanyRepository>();
			services.AddScoped<IJobRepository,JobRepository>();
			services.AddScoped<IApplicationRepository, ApplicationRepository>();
			services.AddScoped<ICVRepository, CVRepository>();
			services.AddScoped<ISkillRepository, SkillRepository>();

			

			// Unit of Work
			services.AddScoped<IUnitOfWork, UnitOfWork>();


			// Services
			services.AddSingleton<IFileStorageService>(_ => new LocalFileStorageService("files"));

			//JWT generator options placeholder
			services.Configure<JwtSettings>(
						configuration.GetSection("JwtSettings"));

			services.AddScoped<IJwtTokenService, JwtTokenService>();
			services.AddScoped<IIdentityService, IdentityService>();
			services.AddScoped<IFileService, CloudinaryFileService>();


			services.Configure<CloudinarySettings>(
						configuration.GetSection("CloudinarySettings"));

			services.AddSingleton(provider =>
			{
				var config = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;

				var account = new Account(
					config.CloudName,
					config.ApiKey,
					config.ApiSecret
				);

				return new Cloudinary(account);
			});
			
			// Background Service
			//services.AddHostedService<OutboxProcessor>();

			// Infrastructure/DependencyInjection.cs
			services.AddHttpClient<IAiScoringService, AiScoringService>(client =>
			{
				client.BaseAddress = new Uri(configuration["AiService:BaseUrl"]!);
				client.Timeout = TimeSpan.FromMinutes(5); // AI calls ممكن تاخد وقت
			});




			// Email Settings
			services.Configure<EmailSettings>(
			configuration.GetSection("EmailSettings"));

			// Email Sender
			services.AddScoped<IEmailSender, SmtpEmailSender>();

			//// Domain Event Handlers (MediatR بيعمل scan أوتوماتيك)
			//services.AddMediatR(cfg =>
			//	cfg.RegisterServicesFromAssembly(typeof(UserEmailConfirmedDomainEventHandler).Assembly));

			return services;
		}
	}
}
