using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Companies.Command.AddEmployee
{
	public record AddEmployeeCommand(
	string CompanyId,
	string EmployeeId) : ICommand;
}
