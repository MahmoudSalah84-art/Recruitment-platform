using Jobs.Domain.Entities;
using Jobs.Infrastructure.Data.Configurations;
using Jobs.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Jobs.Infrastructure.Data
{
    public class JobDbContext : DbContext
	{

		private readonly SaveChangesInterceptor? _interceptor;

		public JobDbContext(DbContextOptions<JobDbContext> options) : base(options) { }
		


		public DbSet<Company> Companies => Set<Company>();
		public DbSet<CV> CVs => Set<CV>();
		public DbSet<CVJobRecommendation> CVJobRecommendations => Set<CVJobRecommendation>();
		public DbSet<Job> Jobs => Set<Job>();
		public DbSet<JobApplication> JobApplications => Set<JobApplication>();
		public DbSet<JobSkill> JobSkills => Set<JobSkill>();
		public DbSet<Skill> Skills => Set<Skill>();
		public DbSet<User> Users => Set<User>();
		public DbSet<UserSkill> UserSkills => Set<UserSkill>();
		public DbSet<UserExperience> UserExperations => Set<UserExperience>();
		public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new CompanyConfiguration());
			modelBuilder.ApplyConfiguration(new JobConfiguration());
			modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
			modelBuilder.ApplyConfiguration(new SkillConfiguration());
			modelBuilder.ApplyConfiguration(new UserSkillConfiguration());
			modelBuilder.ApplyConfiguration(new JobSkillConfiguration());
			modelBuilder.ApplyConfiguration(new CVJobRecommendationConfiguration());
			modelBuilder.ApplyConfiguration(new CVConfiguration());
			modelBuilder.ApplyConfiguration(new UserExperienceConfiguration());

			modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());

			base.OnModelCreating(modelBuilder);
		}

		public override int SaveChanges()
		{
			UpdateTimestamps();
			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			UpdateTimestamps();
			return base.SaveChanges();
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
	}
}

