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
			builder.Property(us => us.UserId)
				.IsRequired()
				.HasMaxLength(36);

			builder.Property(us => us.SkillId)
				.IsRequired()
				.HasMaxLength(36);

			// ===== Soft Delete =====
			builder.Property(us => us.IsDeleted)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(us => us.DeletedAt)
				.IsRequired(false);

			builder.HasQueryFilter(us => !us.IsDeleted);

			// ===== Relationships =====
			 
			builder.HasOne(us => us.User)
				.WithMany(u => u.Skills)
				.HasForeignKey(us => us.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(us => us.Skill)
				.WithMany(s => s.UserSkills)
				.HasForeignKey(us => us.SkillId)
				.OnDelete(DeleteBehavior.Restrict);


			// ===== Indexes =====
			builder.HasIndex(us => new { us.UserId, us.SkillId })
				.IsUnique()
				.HasFilter("[IsDeleted] = 0");



 
			 
		}
	}
}
