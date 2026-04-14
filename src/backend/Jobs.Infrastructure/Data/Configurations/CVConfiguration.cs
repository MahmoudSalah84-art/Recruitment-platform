using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobs.Infrastructure.Data.Configurations
{
    public class CVConfiguration : IEntityTypeConfiguration<CV>
    {
		public void Configure(EntityTypeBuilder<CV> builder)
		{

			builder.ToTable("CVs");

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
			builder.Property(cv => cv.UserId)
				.IsRequired()
				.HasMaxLength(36);

			builder.Property(cv => cv.Title)
				.HasMaxLength(200)
				.HasDefaultValue(string.Empty);

			builder.OwnsOne(cv => cv.FilePath, fp =>
			{
				fp.Property(f => f.Value)
					.HasColumnName("FilePath")
					.IsRequired()
					.HasMaxLength(500);
			});

			builder.Property(cv => cv.SummaryText)
				.HasMaxLength(3000)
				.IsRequired(false);

			builder.OwnsOne(cv => cv.ParsedData, parsed =>
			{
				parsed.Property(p => p.Json)
					.HasColumnName("ParsedJson")
					.HasColumnType("nvarchar(max)")
					.IsRequired(false);

			});

			// ===== Soft Delete =====
			builder.Property(cv => cv.IsDeleted)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(cv => cv.DeletedAt)
				.IsRequired(false);

			builder.HasQueryFilter(cv => !cv.IsDeleted);

			// ===== Relationships =====
			builder.HasOne(cv => cv.User)
				.WithOne(u => u.CV)
				.HasForeignKey<CV>(cv => cv.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(cv => cv.Applications)
				.WithOne(a => a.CV)
				.HasForeignKey(a => a.CvId)
				.OnDelete(DeleteBehavior.SetNull)
				.IsRequired(false);

			builder.HasMany(cv => cv.CVJobRecommendations)
				.WithOne(r => r.CV)
				.HasForeignKey(r => r.CvId)
				.OnDelete(DeleteBehavior.Cascade);

			// ===== Indexes =====
			builder.HasIndex(cv => cv.UserId).IsUnique();


 


  

		}
	}
}
