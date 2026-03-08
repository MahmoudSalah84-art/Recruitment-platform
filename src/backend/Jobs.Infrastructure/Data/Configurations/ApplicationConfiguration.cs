using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
 
namespace Jobs.Infrastructure.Data.Configurations
{
	public class ApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
	{
		public void Configure(EntityTypeBuilder<JobApplication> builder)
		{
			builder.ToTable("JobApplications");

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
			builder.Property(ja => ja.ApplicantId)
				.IsRequired()
				.HasMaxLength(36);

			builder.Property(ja => ja.JobId)
				.IsRequired()
				.HasMaxLength(36);

			builder.Property(ja => ja.CvId)
				.HasMaxLength(36)
				.IsRequired(false);

			builder.Property(ja => ja.MatchScore)
				.IsRequired()
				.HasDefaultValue(0);

			builder.Property(ja => ja.Status)
				.IsRequired()
				.HasConversion<string>()
				.HasMaxLength(50);

			builder.Property(ja => ja.StatusHistory)
				.IsRequired();

			// ===== Soft Delete =====
			builder.Property(ja => ja.IsDeleted)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(ja => ja.DeletedAt)
				.IsRequired(false);

			builder.HasQueryFilter(ja => !ja.IsDeleted);

			// ===== Relationships =====
			 
			builder.HasOne(a => a.Applicant)
				.WithMany(u => u.Applications)
				.HasForeignKey(a => a.ApplicantId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(a => a.Job)
				.WithMany(j => j.Applications)
				.HasForeignKey(a => a.JobId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(a => a.CV)
				.WithMany(cv => cv.Applications)
				.HasForeignKey(a => a.CvId)
				.OnDelete(DeleteBehavior.SetNull)
				.IsRequired(false);


			// ===== Indexes =====
			builder.HasIndex(ja => ja.ApplicantId);
			builder.HasIndex(ja => ja.JobId);
			// Prevent duplicate applications
			builder.HasIndex(ja => new { ja.ApplicantId, ja.JobId })
				.IsUnique()
				.HasFilter("[IsDeleted] = 0");
   
		}
    }
}