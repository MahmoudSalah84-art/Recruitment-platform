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
			builder.Property(c => c.Id)
				   .ValueGeneratedNever();

			builder.Property(c => c.Name)
				   .HasMaxLength(100)
				   .IsRequired();


			builder.Property(c => c.Industry)
				   .HasMaxLength(150)
				   .IsRequired();

			builder.Property(c => c.Description)
				   .HasMaxLength(4000);

			builder.Property(c => c.LogoUrl)
				   .HasMaxLength(500);

			builder.Property(c => c.IsDeleted)
				   .HasDefaultValue(false);

			builder.Property(c => c.DeletedAt);

			builder.HasQueryFilter(c => !c.IsDeleted);

			builder.OwnsOne(c => c.CompanyAddress, address =>
			{
				address.Property(a => a.Country)
					   .HasMaxLength(50)
					   .IsRequired();
				address.Property(a => a.City)
					   .HasMaxLength(50)
					   .IsRequired();
				address.Property(a => a.Street)
					   .HasMaxLength(100)
					   .IsRequired();
				address.WithOwner();
			});

			builder.HasMany<User>("_employees")
				   .WithOne(j => j.Company)
				   .HasForeignKey(u => u.CompanyId)
				   .OnDelete(DeleteBehavior.Restrict);
			builder.Navigation("_employees")
				   .UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.HasMany<Job>("_jobs")
				   .WithOne(j => j.Company)
				   .HasForeignKey(j => j.CompanyId)
				   .OnDelete(DeleteBehavior.Cascade);
			builder.Navigation("_jobs")
				   .UsePropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
