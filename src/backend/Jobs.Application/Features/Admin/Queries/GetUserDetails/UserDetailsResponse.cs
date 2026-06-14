using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Admin.Queries.GetUserDetails
{

	public record UserDetailsResponse(
		string Id,
		string FirstName,
		string LastName,
		string Email,
		string UserName,
		IList<string> Roles,
		bool IsActive,
		int ApplicationsCount,
		int JobsPostedCount);
}
