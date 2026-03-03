using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Jobs.Infrastructure.Data.Configurations
{
	public class ApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
	{
		public void Configure(EntityTypeBuilder<JobApplication> builder)
		{
			builder.ToTable("JobApplications");

			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id)
				   .ValueGeneratedNever();

			builder.Property(x => x.CreatedAt)
				   .IsRequired();
			builder.Property(x => x.UpdatedAt)
				   .IsRequired(false);

			builder.Property(x => x.MatchScore)
				   .IsRequired();
			builder.ToTable(t => t.HasCheckConstraint("CK_JobApplication_MatchScore", "[MatchScore] >= 0 AND [MatchScore] <= 100"));

			builder.Property(x => x.Status)
				   .HasConversion<string>()
				   .HasMaxLength(50);

			builder.Property(x => x.StatusHistory)
				   .IsRequired(false);

			builder.HasOne(x => x.Applicant)
				   .WithMany() 
				   .HasForeignKey(x => x.ApplicantId)
				   .OnDelete(DeleteBehavior.Restrict); // forbidden delete if there are related applications

			builder.HasOne(x => x.Job)
				   .WithMany()
				   .HasForeignKey(x => x.JobId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.CV)
				   .WithMany()
				   .HasForeignKey(x => x.CvId)
				   .IsRequired(false)
				   .OnDelete(DeleteBehavior.SetNull);

			builder.HasQueryFilter(p => !p.IsDeleted);
		}
    }
}