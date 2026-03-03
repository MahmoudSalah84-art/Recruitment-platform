using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Companies.Queries.GetCompanies;
 
namespace Jobs.Application.Features.Companies.Queries.GetCompanyById
{
	public record GetCompanyByIdQuery(Guid Id) : IQuery<CompanyDto?>;
}
