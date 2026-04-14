using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobs.Infrastructure.Data.Configurations
{
	public class JobConfiguration : IEntityTypeConfiguration<Job>
	{
		public void Configure(EntityTypeBuilder<Job> builder)
		{
			builder.ToTable("Jobs");

			builder.HasKey(e => e.Id);

			builder.Property(e => e.Id)
				.ValueGeneratedNever()
				.IsRequired()
				.HasMaxLength(36);

			builder.Property(e => e.CreatedAt)
				.IsRequired();

			builder.Property(e => e.UpdatedAt)
				.IsRequired(false);

			// ===== Properties =====
			builder.Property(j => j.CompanyId)
				.IsRequired()
				.HasMaxLength(36);

			builder.Property(j => j.HrId)
				.IsRequired()
				.HasMaxLength(36);

			builder.Property(j => j.Title)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(j => j.Description)
				.IsRequired()
				.HasColumnType("nvarchar(max)");

			builder.Property(j => j.Requirements)
				.IsRequired()
				.HasColumnType("nvarchar(max)");

			builder.Property(j => j.EmploymentType)
				.IsRequired()
				.HasConversion<string>()
				.HasMaxLength(50);

			builder.Property(j => j.ExperienceLevel)
				.IsRequired()
				.HasDefaultValue(0);

			builder.OwnsOne(j => j.Salary, salary =>
			{
				salary.Property(s => s.Min)
					.HasColumnName("SalaryMin")
					.IsRequired(false);

				salary.Property(s => s.Max)
					.HasColumnName("SalaryMax")
					.IsRequired(false);
			});
		 
			builder.Property(j => j.IsPublished)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(j => j.ExpirationDate)
				.IsRequired(false);

			// Ignored computed property
			builder.Ignore(j => j.IsExpired);

			// ===== Soft Delete =====
			builder.Property(j => j.IsDeleted)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(j => j.DeletedAt)
				.IsRequired(false);

			builder.HasQueryFilter(j => !j.IsDeleted);

			// ===== Relationships =====
	   
			builder.HasOne(j => j.Company)
				.WithMany(c => c.Jobs)
				.HasForeignKey(j => j.CompanyId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(j => j.HR)
				.WithMany()
				.HasForeignKey(j => j.HrId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(j => j.RequiredSkills)
				.WithOne(js => js.Job)
				.HasForeignKey(js => js.JobId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(j => j.Applications)
				.WithOne(a => a.Job)
				.HasForeignKey(a => a.JobId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(j => j.CVJobRecommendations)
				.WithOne(r => r.Job)
				.HasForeignKey(r => r.JobId)
				.OnDelete(DeleteBehavior.Cascade);

			// ===== Indexes =====
			builder.HasIndex(j => j.CompanyId);
			builder.HasIndex(j => j.HrId);
			builder.HasIndex(j => j.IsPublished);
			builder.HasIndex(j => j.ExpirationDate);





   
 
		}
	}
}
