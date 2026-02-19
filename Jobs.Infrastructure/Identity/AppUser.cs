using Microsoft.AspNetCore.Identity;

namespace Jobs.Infrastructure.Identity
{
	public class AppUser : IdentityUser<Guid>
	{
		public string? FullName { get; set; }
	}
}
