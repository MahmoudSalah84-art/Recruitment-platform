using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Identity
{
	public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, string>
	{
		public DbSet<RefreshToken> RefreshTokens { get; set; }

		public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<AppUser>().ToTable("AspNetUsers");
			builder.Entity<AppRole>().ToTable("AspNetRoles");
			builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
			builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
			builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
			builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");
			builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
			builder.Entity<RefreshToken>().ToTable("AspNetRefreshToken");


			builder.Entity<RefreshToken>(e =>
			{
				e.HasKey(x => x.Id);
				e.HasOne(x => x.AppUser)
				 .WithMany(u => u.RefreshTokens)
				 .HasForeignKey(x => x.UserId)
				 .OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}
