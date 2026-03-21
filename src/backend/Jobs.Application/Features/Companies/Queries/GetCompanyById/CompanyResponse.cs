using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Companies.Queries.GetCompanyById
{
	public record CompanyResponse(
	string Id,
	string Name,
	string Email,
	string Industry,
	string Street,
	string City,
	string Country,
	int EmployeesCount,
	string Description,
	string LogoUrl,
	int ActiveJobsCount,
	int TotalEmployeesCount);
}
