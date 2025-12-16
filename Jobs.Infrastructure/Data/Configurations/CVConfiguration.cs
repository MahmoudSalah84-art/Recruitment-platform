using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Data.Configurations
{
    public class CVConfiguration
    {
		public void Configure(EntityTypeBuilder<CV> builder)
		{
			builder.ToTable("CVs");

			builder.HasKey(cv => cv.Id);
			builder.Property(cv => cv.Id)
				   .ValueGeneratedNever();

			builder.Property(cv => cv.Title)
				   .HasMaxLength(200)
				   .IsRequired();

			builder.Property(cv => cv.SummaryText)
				   .HasMaxLength(2000);

			builder.HasOne<User>()
				   .WithMany(u => u.CVs)
				   .HasForeignKey(cv => cv.UserId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.OwnsOne(cv => cv.FilePath, fp =>
			{
				fp.Property(f => f.Value)
					.HasColumnName("FilePath")
					.HasMaxLength(500)
					.IsRequired();
				fp.WithOwner(); // tell EF Core that FilePath is owned by CV , if you remove this line, it will create a separate table for FilePath
			});

			
			builder.OwnsOne(cv => cv.ParsedData, parsed =>
			{
				parsed.Property(p => p.Json)
					  .HasColumnName("ParsedJson")
					  .HasColumnType("nvarchar(max)");
				parsed.WithOwner();
			});

			// ===== Soft Delete =====
			builder.Property(cv => cv.IsDeleted)
				   .HasDefaultValue(false);
			builder.Property(cv => cv.DeletedAt);
			builder.HasQueryFilter(cv => !cv.IsDeleted);
		}
	}
}
