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
			builder.Property(s => s.Name).HasMaxLength(200).IsRequired();

			builder.HasIndex(s => s.Name).IsUnique();

			builder.HasQueryFilter(p => !p.IsDeleted);

		}
	}
}
