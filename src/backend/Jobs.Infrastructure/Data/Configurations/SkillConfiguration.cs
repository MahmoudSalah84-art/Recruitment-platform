using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Data.Configurations
{
	public class SkillConfiguration : IEntityTypeConfiguration<Skill>
	{
		public void Configure(EntityTypeBuilder<Skill> builder)
		{
			builder.ToTable("Skills");

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
			builder.Property(s => s.Name)
				.IsRequired()
				.HasMaxLength(100);

			// ===== Soft Delete =====
			builder.Property(s => s.IsDeleted)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(s => s.DeletedAt)
				.IsRequired(false);

			builder.HasQueryFilter(s => !s.IsDeleted);

			 

			// ===== Indexes =====
			builder.HasIndex(s => s.Name)
				.IsUnique()
				.HasFilter("[IsDeleted] = 0");


 
			 

		}
	}
}
