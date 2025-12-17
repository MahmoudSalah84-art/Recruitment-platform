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

			builder.HasKey(r => r.Id);

			builder.Property(r => r.Id)
				   .ValueGeneratedNever();

			builder.Property(r => r.Score)
				   .HasPrecision(5, 4)
				   .IsRequired();

			builder.Property(r => r.CreatedAt)
				   .IsRequired();

			builder.Property(r => r.IsActive)
				   .HasDefaultValue(true);

			builder.HasOne(r => r.CV)
				   .WithMany()
				   .HasForeignKey(r => r.CvId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(r => r.Job)
				   .WithMany()
				   .HasForeignKey(r => r.JobId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(r => new { r.CvId, r.JobId })
				   .IsUnique();

			builder.HasQueryFilter(p => !p.IsDeleted);

		}
	}
}
