using Jobs.Application.Abstractions.Messaging;
 
namespace Jobs.Application.Features.Companies.Commands.DeleteCompany
{
	public record DeleteCompanyCommand(Guid CompanyId) : ICommand;
}
