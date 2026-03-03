using Microsoft.AspNetCore.Identity;

namespace Jobs.Infrastructure.Identity
{
	public class AppRole : IdentityRole
	{
		public AppRole() { }
		public AppRole(string roleName) : base(roleName) { }
		public string? Description { get; set; }
	}

}
