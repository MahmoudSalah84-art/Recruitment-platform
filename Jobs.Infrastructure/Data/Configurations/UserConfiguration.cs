using Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Data.Configurations
{ 
    public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{

			builder.Property<string>("Email") // shadow property used for indexing/unique
				.HasMaxLength(255)
				.IsRequired();


			builder.ToTable("Users");

			builder.HasKey(u => u.Id);
			builder.Property(u => u.Id)
				   .ValueGeneratedNever();

			builder.Property(u => u.FullName)
				   .HasMaxLength(200)
				   .IsRequired();

			builder.Property(u => u.Role)
				   .IsRequired();

			builder.Property(u => u.ProfileImage)
				   .HasMaxLength(500);

			builder.Property(u => u.Bio)
				   .HasMaxLength(2000);

			builder.Property(u => u.IsVerified)
				   .HasDefaultValue(false);

			builder.OwnsOne(u => u.Email, email =>
			{
				email.Property(e => e.Value)
					 .HasColumnName("Email")
					 .HasMaxLength(100)
					 .IsRequired();
				email.WithOwner();
			});

			builder.OwnsOne(u => u.PhoneNumber, phone =>
			{
				phone.Property(p => p.Value)
					 .HasColumnName("PhoneNumber")
					 .HasMaxLength(30);
				phone.WithOwner();
			});


			builder.HasOne(u => u.Company)
				   .WithMany("_employees")
				   .HasForeignKey(u => u.CompanyId)
				   .OnDelete(DeleteBehavior.Restrict);
			builder.Navigation("_employees")
				   .UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.HasMany<UserSkill>("_skills")
				   .WithOne(us => us.User)
				   .HasForeignKey(us => us.UserId)
				   .OnDelete(DeleteBehavior.Cascade);
			builder.Navigation("_skills")
				   .UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.HasMany<CV>("_cvs")
				   .WithOne()
				   .HasForeignKey(cv => cv.UserId)
				   .OnDelete(DeleteBehavior.Cascade);
			builder.Navigation("_cvs")
				   .UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.HasMany<JobApplication>("_applications")
				   .WithOne(a => a.Applicant)
				   .HasForeignKey(a => a.ApplicantId)
				   .OnDelete(DeleteBehavior.Cascade);
			builder.Navigation("_applications")
				   .UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.Property(u => u.IsDeleted)
				   .HasDefaultValue(false);

			builder.Property(u => u.DeletedAt);

			builder.HasQueryFilter(u => !u.IsDeleted);

			// ===== Indexes =====
			builder.HasIndex(u => u.CompanyId);
			builder.HasIndex(u => u.Role);
			builder.HasIndex("Email")
				   .IsUnique();
		}
	}
}
