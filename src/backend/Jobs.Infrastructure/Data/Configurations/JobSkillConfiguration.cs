using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Jobs.Infrastructure.Data.Configurations
{
	public class JobSkillConfiguration : IEntityTypeConfiguration<JobSkill>
	{
		public void Configure(EntityTypeBuilder<JobSkill> builder)
		{
			builder.ToTable("JobSkills");

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
			builder.Property(js => js.JobId)
				.IsRequired()
				.HasMaxLength(36);

			builder.Property(js => js.SkillId)
				.IsRequired()
				.HasMaxLength(36);

			// ===== Soft Delete =====
			builder.Property(js => js.IsDeleted)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(js => js.DeletedAt)
				.IsRequired(false);

			builder.HasQueryFilter(js => !js.IsDeleted);

			// ===== Relationships =====
			 
			builder.HasOne(js => js.Job)
				.WithMany(j => j.RequiredSkills)
				.HasForeignKey(js => js.JobId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(js => js.Skill)
				.WithMany(s => s.JobSkills)
				.HasForeignKey(js => js.SkillId)
				.OnDelete(DeleteBehavior.Restrict);

			// ===== Indexes =====
			builder.HasIndex(js => new { js.JobId, js.SkillId })
				.IsUnique()
				.HasFilter("[IsDeleted] = 0");



			
			 
			 




		}
	}
}
