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
			builder.ToTable("Users");

			builder.HasKey(u => u.Id);
			builder.Property(u => u.Id).ValueGeneratedNever();

			builder.Property(u => u.FullName)
				.HasMaxLength(200)
				.IsRequired();

			builder.Property<string>("Email") // shadow property used for indexing/unique
				.HasMaxLength(255)
				.IsRequired();

			builder.HasIndex("Email").IsUnique();

			builder.Property(u => u.ProfileImage)
				.HasMaxLength(1000);

			builder.Property(u => u.Bio)
				.HasMaxLength(2000);

			builder.Property<Guid?>("IdentityUserId")
				.HasColumnName("IdentityUserId")
				.IsRequired(false);

			// Owned collections or relationships
			builder.Navigation(u => u.CVs).HasField("_cvs");
			builder.Navigation(u => u.Skills).HasField("_skills");
			builder.Navigation(u => u.Applications).HasField("_applications");

			builder.HasQueryFilter(p => !p.IsDeleted);

		}
	}
}
