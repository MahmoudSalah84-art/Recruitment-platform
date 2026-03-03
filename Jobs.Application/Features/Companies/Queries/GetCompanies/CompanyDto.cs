
namespace Jobs.Application.Features.Companies.Queries.GetCompanies
{
	public record CompanyDto(
	Guid Id,
	string Name,
	string Description,
	string Industry,
	int EmployeesCount,
	string LogoUrl,

	// Address
	string Country,
	string City,
	string Street,
	string BuildingNumber,
	string PostalCode
	);
	
}
