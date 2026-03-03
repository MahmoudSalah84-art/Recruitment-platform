using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Companies.Queries.GetCompanyProfile;

namespace Jobs.Application.Features.Companies.Queries.GetCompanyDetails
{
	public record GetCompanyProfileQuery() : IQuery<CompanyProfileDto>;
}
