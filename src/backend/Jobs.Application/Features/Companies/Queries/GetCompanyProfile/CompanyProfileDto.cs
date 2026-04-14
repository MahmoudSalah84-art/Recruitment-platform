
namespace Jobs.Application.Features.Companies.Queries.GetCompanyProfile
{
	public record CompanyProfileDto(
	string Id,
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
