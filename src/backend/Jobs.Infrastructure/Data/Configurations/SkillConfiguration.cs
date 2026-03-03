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

			builder.HasKey(s => s.Id);
			builder.Property(s => s.Id)
				   .ValueGeneratedNever();

			builder.Property(s => s.Name)
				   .HasMaxLength(150)
				   .IsRequired();

			builder.Property(s => s.IsDeleted)
				   .HasDefaultValue(false);

			builder.Property(s => s.DeletedAt);

			builder.HasQueryFilter(s => !s.IsDeleted);

			// ===== Indexes =====
			builder.HasIndex(s => s.Name)
				   .IsUnique();

		}
	}
}
