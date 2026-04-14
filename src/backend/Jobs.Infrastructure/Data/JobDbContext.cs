using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Jobs.Infrastructure.Data.Configurations;
using Jobs.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Infrastructure.Data
{
    public class JobDbContext : DbContext
	{
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
		public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Ignore<DomainEvent>();

			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new CompanyConfiguration());
			modelBuilder.ApplyConfiguration(new JobConfiguration());
			modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
			modelBuilder.ApplyConfiguration(new SkillConfiguration());
			modelBuilder.ApplyConfiguration(new UserSkillConfiguration());
			modelBuilder.ApplyConfiguration(new JobSkillConfiguration());
			modelBuilder.ApplyConfiguration(new CVJobRecommendationConfiguration());
			modelBuilder.ApplyConfiguration(new CVConfiguration());

			modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}

