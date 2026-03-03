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

			builder.HasKey(js => js.Id);
			builder.Property(js => js.Id)
				   .ValueGeneratedNever();

			builder.HasOne(js => js.Job)
				   .WithMany()
				   .HasForeignKey(js => js.JobId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(js => js.Skill)
				   .WithMany()
				   .HasForeignKey(js => js.SkillId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.Property(js => js.IsDeleted)
				   .HasDefaultValue(false);

			builder.Property(js => js.DeletedAt);

			builder.HasQueryFilter(js => !js.IsDeleted);



			// ===== Indexes =====
			builder.HasIndex(js => new { js.JobId, js.SkillId }) // Composite index on JobId and SkillId
				   .IsUnique();

		}
	}
}
