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
			builder.ToTable("Applications");

			builder.HasKey(a => a.Id);
			builder.Property(a => a.Id).ValueGeneratedNever();

			builder.Property(a => a.MatchScore).HasPrecision(5, 4);

			builder.Property<string>("Status") // map enum or string
				.HasMaxLength(50)
				.IsRequired();

			builder.OwnsOne(a => a.StatusHistory, sh =>
			{
				sh.Property(p => p.OldStatus)
				  .HasColumnName("OldStatus");

				sh.Property(p => p.NewStatus)
				  .HasColumnName("NewStatus");

				sh.Property(p => p.ChangedAt)
				  .HasColumnName("StatusChangedAt");
			});

			builder.HasOne<Job>()
				.WithMany()
				.HasForeignKey(a => a.JobId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey(a => a.ApplicantId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasQueryFilter(p => !p.IsDeleted);

		}

       
    }
}