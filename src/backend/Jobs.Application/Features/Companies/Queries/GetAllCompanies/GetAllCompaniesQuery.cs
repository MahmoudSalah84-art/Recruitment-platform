using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Companies.Queries.GetCompanyById;

namespace Jobs.Application.Features.Companies.Queries.GetAllCompanies
{
	public record GetAllCompaniesQuery(
	int Page = 1,
	int PageSize = 10,
	string? Name = null,
	string? Industry = null) : IQuery<PaginatedList<CompanyResponse>>;

}
