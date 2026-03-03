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

			builder.HasKey(j => j.Id);
			builder.Property(j => j.Id)
				   .ValueGeneratedNever();

			builder.Property(j => j.Title)
				   .HasMaxLength(200)
				   .IsRequired();

			builder.Property(j => j.Description)
				   .HasMaxLength(4000)
				   .IsRequired();

			builder.Property(j => j.Requirements)
				   .HasMaxLength(4000);

			builder.Property(j => j.ExperienceLevel)
				   .IsRequired();

			builder.Property(j => j.IsPublished)
				   .HasDefaultValue(false);

			builder.Property(j => j.ExpirationDate);

			builder.HasOne(j => j.Company)
				   .WithMany("_jobs")
				   .HasForeignKey(j => j.CompanyId)
				   .OnDelete(DeleteBehavior.Cascade);
			builder.Navigation("_jobs")
				   .UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.HasOne(j => j.Hr)
				   .WithMany()
				   .HasForeignKey(j => j.HrId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.OwnsOne(j => j.Salary, salary =>
			{
				salary.Property(s => s.Min)
					  .HasColumnName("SalaryMin")
					  .HasMaxLength(10);
				salary.Property(s => s.Max)
					  .HasColumnName("SalaryMax")
					  .HasMaxLength(10);
				salary.WithOwner();
			});

			builder.HasMany<JobSkill>("_requiredSkills")
				   .WithOne(js => js.Job)
				   .HasForeignKey(js => js.JobId)
				   .OnDelete(DeleteBehavior.Cascade);
			builder.Navigation("_requiredSkills")
				   .UsePropertyAccessMode(PropertyAccessMode.Field);


			builder.Property(j => j.IsDeleted)
				   .HasDefaultValue(false);

			builder.Property(j => j.DeletedAt);

			builder.HasQueryFilter(j => !j.IsDeleted);

			// ===== Indexes =====
			builder.HasIndex(j => j.CompanyId);
			builder.HasIndex(j => j.IsPublished);
			builder.HasIndex(j => j.ExpirationDate);
		}
	}
}
