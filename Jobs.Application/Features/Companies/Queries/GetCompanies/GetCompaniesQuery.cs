using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Companies.Queries.GetCompanies
{
	public record GetCompaniesQuery() : IQuery<List<CompanyDto>>;
}
