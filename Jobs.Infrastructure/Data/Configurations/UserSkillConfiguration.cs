using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Data.Configurations
{
	public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
	{
		public void Configure(EntityTypeBuilder<UserSkill> builder)
		{
			builder.ToTable("UserSkills");

			builder.HasKey(us => us.Id);

			builder.Property(us => us.Id)
				   .ValueGeneratedNever();

			builder.HasOne(us => us.User)
				   .WithMany(u => u.Skills)
				   .HasForeignKey(us => us.UserId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(us => us.Skill)
				   .WithMany()
				   .HasForeignKey(us => us.SkillId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(us => new { us.UserId, us.SkillId })
				   .IsUnique();

			builder.HasQueryFilter(p => !p.IsDeleted);

		}
	}
}
