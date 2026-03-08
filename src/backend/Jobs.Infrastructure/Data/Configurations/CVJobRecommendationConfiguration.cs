using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Data.Configurations
{
	public class CVJobRecommendationConfiguration : IEntityTypeConfiguration<CVJobRecommendation>
	{
		public void Configure(EntityTypeBuilder<CVJobRecommendation> builder)
		{
			builder.ToTable("CVJobRecommendations");

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
			builder.Property(r => r.CvId)
				.IsRequired()
				.HasMaxLength(36);

			builder.Property(r => r.JobId)
				.IsRequired()
				.HasMaxLength(36);

			builder.Property(r => r.Score)
				.IsRequired()
				.HasDefaultValue(0);
			
			builder.Property(r => r.IsActive)
				.IsRequired()
				.HasDefaultValue(true);

			builder.Property(r => r.DeactivatedAt)
				.IsRequired(false);

			// ===== Soft Delete =====
			builder.Property(r => r.IsDeleted)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(r => r.DeletedAt)
				.IsRequired(false);

			builder.HasQueryFilter(r => !r.IsDeleted);

			// ===== Relationships =====
			 
			builder.HasOne(r => r.CV)
				.WithMany(cv => cv.CVJobRecommendations)
				.HasForeignKey(r => r.CvId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(r => r.Job)
				.WithMany(j => j.CVJobRecommendations)
				.HasForeignKey(r => r.JobId)
				.OnDelete(DeleteBehavior.Restrict);

			// ===== Indexes =====
			builder.HasIndex(r => r.CvId);
			builder.HasIndex(r => r.JobId);
			builder.HasIndex(r => new { r.CvId, r.JobId })
				.IsUnique()
				.HasFilter("[IsDeleted] = 0");
			builder.HasIndex(r => r.Score);








		

		 

		}
	}
}
