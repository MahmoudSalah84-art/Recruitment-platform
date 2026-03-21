using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Companies.Command.UpdateCompany
{
	public record UpdateCompanyCommand(
	string CompanyId,
	string Industry,
	string Country,
	string City,
	string Street,
	string BuildingNumber,
	string PostalCode,
	int EmployeesCount,
	string? Description) : ICommand;
}
