using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Data.Configurations
{
    internal class UserExperienceConfiguration : IEntityTypeConfiguration<UserExperience>
	{
		public void Configure(EntityTypeBuilder<UserExperience> builder)
		{
			// ========= Table =========
			builder.ToTable("UserExperiences");

			// ========= Key =========
			builder.HasKey(x => x.Id);

			// ========= Properties =========
			builder.Property(x => x.JobTitle)
				   .IsRequired()
				   .HasMaxLength(150);

			builder.Property(x => x.Description)
				   .HasMaxLength(2000);

			builder.Property(x => x.EmploymentType)
				   .IsRequired()
				   .HasConversion<int>();

			builder.Property(x => x.StartDate)
				   .IsRequired();

			builder.Property(x => x.EndDate)
				   .IsRequired(false);

			builder.Property(x => x.IsCurrent)
				   .IsRequired();

			// ========= Relationships =========

			// User (1) -> (N) Experiences
			builder.HasOne(x => x.User)
				   .WithMany(u => u.Experience)
				   .HasForeignKey(x => x.UserId)
				   .OnDelete(DeleteBehavior.Cascade);

			// Company (1) -> (N) Experiences
			builder.HasOne(x => x.Company)
				   .WithMany()
				   .HasForeignKey(x => x.CompanyId)
				   .OnDelete(DeleteBehavior.Restrict);

			// ========= Indexes =========
			builder.HasIndex(x => x.UserId);
			builder.HasIndex(x => x.CompanyId);
			builder.HasIndex(x => new { x.UserId, x.IsCurrent });

			// ========= Ignore ( Domain Events) =========
			builder.Ignore("_domainEvents");
		}
	}
}
