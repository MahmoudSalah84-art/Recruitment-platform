using Jobs.Domain.Entities;
using Jobs.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobs.Infrastructure.Data.Configurations
{ 
    public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");

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
			builder.Property(u => u.FirstName)
				.IsRequired()
				.HasMaxLength(150);

			builder.Property(u => u.LastName)
				.IsRequired()
				.HasMaxLength(150);


			builder.OwnsOne(u => u.Email, email =>
			{
				email.Property(e => e.Value)
					.HasColumnName("EmailAddress")
					.IsRequired()
					.HasMaxLength(256);
				email.HasIndex(e => e.Value)
						.IsUnique()
						.HasFilter("[IsDeleted] = 0");

			});

			builder.Property(u => u.Role)
					.IsRequired()
					.HasConversion<string>()
					.HasMaxLength(50);

			builder.OwnsOne(u => u.PhoneNumber, phone =>
			{
				phone.Property(p => p.Value)
					.HasColumnName("PhoneNumber")
					.HasMaxLength(20)
					.IsRequired(false);
			});


			builder.Property(u => u.ProfilePictureUrl)
				.HasMaxLength(500)
				.HasDefaultValue(string.Empty);

			builder.Property(u => u.Bio)
				.HasMaxLength(1000)
				.HasDefaultValue(string.Empty);

			builder.Property(u => u.LinkedInUrl)
				.HasMaxLength(300)
				.IsRequired(false);

			builder.Property(u => u.GitHubUrl)
				.HasMaxLength(300)
				.IsRequired(false);

			builder.Property(u => u.PortfolioUrl)
				.HasMaxLength(300)
				.IsRequired(false);

			builder.Property(u => u.IsVerified)
				.IsRequired()
				.HasDefaultValue(false);

			// ===== Soft Delete =====

			builder.Property(u => u.IsDeleted)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(u => u.DeletedAt)
				.IsRequired(false);

			builder.HasQueryFilter(u => !u.IsDeleted);



			// Relationships
			builder.HasOne(u => u.Company)
				.WithMany(c => c.Employees)
				.HasForeignKey(u => u.CompanyId)
				.OnDelete(DeleteBehavior.SetNull)
				.IsRequired(false);

			builder.HasOne(u => u.CV)
				.WithOne(cv => cv.User)
				.HasForeignKey<CV>(cv => cv.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(u => u.Skills)
				.WithOne(us => us.User)
				.HasForeignKey(us => us.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(u => u.Applications)
				.WithOne(a => a.Applicant)
				.HasForeignKey(a => a.ApplicantId)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
