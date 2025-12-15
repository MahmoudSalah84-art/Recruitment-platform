using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Jobs.Infrastructure.Data.Configurations
{
	public class JobConfiguration : IEntityTypeConfiguration<Job>
	{
		public void Configure(EntityTypeBuilder<Job> builder)
		{
			builder.ToTable("Jobs");

			builder.HasKey(j => j.Id);
			builder.Property(j => j.Id).ValueGeneratedNever();

			builder.Property(j => j.Title)
				.HasMaxLength(300)
				.IsRequired();

			builder.Property(j => j.Description)
				.HasMaxLength(8000);

			builder.OwnsOne(j => j.Salary, s =>
			{
				s.Property(p => p.Min).HasColumnName("SalaryMin");
				s.Property(p => p.Max).HasColumnName("SalaryMax");
			});

			builder.Property(j => j.IsPublished).HasDefaultValue(false);

			builder.HasOne<Company>()
				.WithMany()
				.HasForeignKey(j => j.CompanyId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasQueryFilter(p => !p.IsDeleted);


			// required skills as separate table configured in JobSkillConfiguration
		}
	}
}
