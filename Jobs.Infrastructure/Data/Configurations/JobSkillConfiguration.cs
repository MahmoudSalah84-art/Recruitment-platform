using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Data.Configurations
{
	public class JobSkillConfiguration : IEntityTypeConfiguration<JobSkill>
	{
		public void Configure(EntityTypeBuilder<JobSkill> builder)
		{
			builder.ToTable("JobSkills");

			builder.HasKey(js => js.Id);

			builder.Property(js => js.Id)
				   .ValueGeneratedNever();

			builder.HasOne(js => js.Job)
				   .WithMany(j => j.RequiredSkills)
				   .HasForeignKey(js => js.JobId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(js => js.Skill)
				   .WithMany()
				   .HasForeignKey(js => js.SkillId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(js => new { js.JobId, js.SkillId })
				   .IsUnique();
		}
	}
}
