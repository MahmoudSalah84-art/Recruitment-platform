using Jobs.Application.Abstractions.Messaging;
 
namespace Jobs.Application.Features.Companies.Commands.UpdateCompany
{
	public record UpdateCompanyCommand(
		string Name,
		string Description,
		string Website,
		string Industry,
		int EmployeesCount,
		string LogoUrl,

		// Address fields
		string Country,
		string City,
		string Street,
		string BuildingNumber,
		string PostalCode
	) : ICommand;
}
