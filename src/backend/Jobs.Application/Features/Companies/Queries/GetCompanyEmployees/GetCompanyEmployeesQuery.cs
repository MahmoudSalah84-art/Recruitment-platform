using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;

namespace Jobs.Application.Features.Companies.Queries.GetCompanyEmployees
{
	public record GetCompanyEmployeesQuery(
	string CompanyId,
	int Page = 1,
	int PageSize = 10) : IQuery<PaginatedList<EmployeeResponse>>;
}
