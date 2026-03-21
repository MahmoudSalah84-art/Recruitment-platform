using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Companies.Command.UpdateCompanyLogo
{
	public record UpdateCompanyLogoCommand(
	string CompanyId,
	string LogoUrl) : ICommand;

}
