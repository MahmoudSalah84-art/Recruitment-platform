using Jobs.Domain.Entities;
using Jobs.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Jobs.Infrastructure.Data
{
    public class JobDbContext : DbContext
	{

		private readonly IOutboxPublisher? _outboxPublisher;
		private readonly SaveChangesInterceptor? _interceptor;

		public JobDbContext(DbContextOptions<JobDbContext> options, IOutboxPublisher? outboxPublisher = null, SaveChangesInterceptor? interceptor = null): base(options)
		{
			_outboxPublisher = outboxPublisher;
			_interceptor = interceptor;
		}


		public DbSet<Company> Companies => Set<Company>();
		public DbSet<CV> CVs => Set<CV>();
		public DbSet<CVJobRecommendation> CVJobRecommendations => Set<CVJobRecommendation>();
		public DbSet<Job> Jobs => Set<Job>();
		public DbSet<JobApplication> JobApplications => Set<JobApplication>();
		public DbSet<JobSkill> JobSkills => Set<JobSkill>();
		public DbSet<Skill> Skills => Set<Skill>();
		public DbSet<User> Users => Set<User>();
		public DbSet<UserSkill> UserSkills => Set<UserSkill>();

	

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Apply entity configurations from the Infrastructure assembly
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new CompanyConfiguration());
			modelBuilder.ApplyConfiguration(new JobConfiguration());
			modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
			modelBuilder.ApplyConfiguration(new SkillConfiguration());
			modelBuilder.ApplyConfiguration(new UserSkillConfiguration());
			modelBuilder.ApplyConfiguration(new JobSkillConfiguration());
			modelBuilder.ApplyConfiguration(new CVConfiguration());
			modelBuilder.ApplyConfiguration(new RecommendationConfiguration());

			// Outbox table configuration
			modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());

			base.OnModelCreating(modelBuilder);
		}

		public override int SaveChanges()
		{
			UpdateTimestamps();
			var result = base.SaveChanges();
			DispatchOutboxAsync().GetAwaiter().GetResult();
			return result;
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			UpdateTimestamps();
			var result = await base.SaveChangesAsync(cancellationToken);
			await DispatchOutboxAsync(cancellationToken);
			return result;
		}

		private void UpdateTimestamps()
		{
			var entries = ChangeTracker.Entries()
				.Where(e => e.Entity is not OutboxMessage && (e.State == EntityState.Added || e.State == EntityState.Modified));

			foreach (var entry in entries)
			{
				var propCreated = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "CreatedAt");
				var propUpdated = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "UpdatedAt");

				if (entry.State == EntityState.Added)
				{
					propCreated?.CurrentValue = DateTime.UtcNow;
				}
				propUpdated?.CurrentValue = DateTime.UtcNow;
			}
		}

		private async Task DispatchOutboxAsync(CancellationToken ct = default)
		{
			if (_outboxPublisher == null) return;

			var messages = await OutboxMessages
				.Where(m => !m.Processed)
				.OrderBy(m => m.OccurredOn)
				.ToListAsync(ct);

			foreach (var msg in messages)
			{
				try
				{
					await _outboxPublisher.PublishAsync(msg, ct);
					msg.MarkProcessed();
				}
				catch
				{
					// Logging / retry handled by outbox publisher
				}
			}

			await base.SaveChangesAsync(ct);
		}
	}
}

