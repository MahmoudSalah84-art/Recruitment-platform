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

			builder.HasKey(c => c.Id);
			builder.Property(c => c.Id).ValueGeneratedNever();

			builder.Property(c => c.Title).HasMaxLength(300).IsRequired();
			builder.Property(c => c.SummaryText).HasMaxLength(4000);

			builder.OwnsOne(c => c.FilePath, f =>
			{
				f.Property(p => p.Value).HasColumnName("FilePath").HasMaxLength(2000);
			});

			builder.OwnsOne(c => c.ParsedData, pd =>
			{
				pd.Property(p => p.Json).HasColumnName("ParsedJson");
			});

			builder.HasOne<User>()
				.WithMany(u => u.CVs)
				.HasForeignKey("UserId")
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasQueryFilter(p => !p.IsDeleted);

		}
	}
}
