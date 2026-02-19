using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Infrastructure.Identity
{
	public class IdentityDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
	{
		public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<AppUser>().ToTable("AspNetUsers");
			builder.Entity<IdentityRole<Guid>>().ToTable("AspNetRoles");
			builder.Entity<IdentityUserRole<Guid>>().ToTable("AspNetUserRoles");
			builder.Entity<IdentityUserClaim<Guid>>().ToTable("AspNetUserClaims");
			builder.Entity<IdentityUserLogin<Guid>>().ToTable("AspNetUserLogins");
			builder.Entity<IdentityUserToken<Guid>>().ToTable("AspNetUserTokens");
			builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AspNetRoleClaims");
		}
	}

}
