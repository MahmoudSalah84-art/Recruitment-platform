using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Data.Configurations
{
    internal class CompanyConfiguration : IEntityTypeConfiguration<Company>
	{
		public void Configure(EntityTypeBuilder<Company> builder)
		{
			builder.ToTable("Companies");

			builder.HasKey(c => c.Id);
			builder.Property(c => c.Id).ValueGeneratedNever();

			builder.Property(c => c.Name)
				.HasMaxLength(200)
				.IsRequired();

			builder.Property(c => c.Industry)
				.HasMaxLength(200);

			builder.Property(c => c.Description)
				.HasMaxLength(4000);

			builder.Property(c => c.CompanyAddress)//////////
				.HasMaxLength(400);

			builder.Property(c => c.LogoUrl)
				.HasMaxLength(1000);

			builder.HasQueryFilter(p => !p.IsDeleted);

		}
	}
}
