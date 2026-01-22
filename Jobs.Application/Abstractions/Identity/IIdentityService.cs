using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Abstractions.Identity
{
	public interface IIdentityService
	{
		Task<Guid> CreateUserAsync(string email, string password, string fullName);
	}
}
