

using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Companies.Command.DeleteCompany
{
	public record DeleteCompanyCommand(string CompanyId) : ICommand;

}
