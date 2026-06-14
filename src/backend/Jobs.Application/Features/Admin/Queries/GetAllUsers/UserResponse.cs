using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Admin.Queries.GetAllUsers
{
	public record UserResponse(
	string Id,
	string FirstName,
	string LastName,
	string Email,
	string? PhoneNumber,
	IList<string> Roles,
	bool IsActive,
	DateTime CreatedAt);
}
