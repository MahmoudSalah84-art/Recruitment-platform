using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Companies.Queries.GetCompanyEmployees
{
	public record EmployeeResponse(string Id, string FirstName, string LastName, string Email);

}
