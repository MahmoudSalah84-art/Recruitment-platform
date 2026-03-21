using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Companies.Command.RemoveEmployee
{
	public record RemoveEmployeeCommand(
	string CompanyId,
	string EmployeeId) : ICommand;
}
