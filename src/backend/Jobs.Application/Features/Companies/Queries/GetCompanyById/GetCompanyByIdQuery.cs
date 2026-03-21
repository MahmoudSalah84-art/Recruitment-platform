using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Companies.Queries.GetCompanyById
{
	public record GetCompanyByIdQuery(string CompanyId) : IQuery<CompanyResponse>;

}
