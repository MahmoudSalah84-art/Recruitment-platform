using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Identity
{
	public class AppUser : IdentityUser<Guid>
	{
		public string? FullName { get; set; }
	}
}
