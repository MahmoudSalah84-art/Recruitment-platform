
namespace Jobs.Application.Features.Companies.Queries.GetCompanyProfile
{
	public record CompanyProfileDto(
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
