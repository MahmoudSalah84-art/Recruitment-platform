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
			builder.Property(c => c.Name)
				.IsRequired()
				.HasMaxLength(200);

			builder.OwnsOne(c => c.Email, email =>
			{
				email.Property(e => e.Value)
					.HasColumnName("EmailAddress")
					.IsRequired()
					.HasMaxLength(256);
			});

			builder.Property(c => c.Industry)
				.IsRequired()
				.HasMaxLength(100);

			builder.OwnsOne(c => c.CompanyAddress, address =>
			{
				address.Property(a => a.Street)
					.HasColumnName("Street")
					.HasMaxLength(200)
					.IsRequired(false);

				address.Property(a => a.City)
					.HasColumnName("City")
					.HasMaxLength(100)
					.IsRequired(false);

				address.Property(a => a.Country)
					.HasColumnName("Country")
					.HasMaxLength(100)
					.IsRequired(false);
			});

			builder.Property(c => c.EmployeesCount)
				.IsRequired()
				.HasDefaultValue(0);

			builder.Property(c => c.Description)
				.HasMaxLength(2000)
				.HasDefaultValue(string.Empty);

			builder.Property(c => c.LogoUrl)
				.HasMaxLength(500)
				.HasDefaultValue(string.Empty);

			// ===== Soft Delete =====
			builder.Property(c => c.IsDeleted)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(c => c.DeletedAt)
				.IsRequired(false);

			builder.HasQueryFilter(c => !c.IsDeleted);
 


			// Relationships
			builder.HasMany(c => c.Jobs)
				.WithOne(j => j.Company)
				.HasForeignKey(j => j.CompanyId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(c => c.Employees)
				.WithOne(u => u.Company)
				.HasForeignKey(u => u.CompanyId)
				.OnDelete(DeleteBehavior.SetNull);



			// ===== Indexes =====
			builder.HasIndex(c => c.Name);
			builder.HasIndex(c => c.Industry);
		}
	}
}
