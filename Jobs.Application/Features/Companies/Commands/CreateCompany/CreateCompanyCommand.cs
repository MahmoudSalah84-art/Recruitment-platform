using Azure.Core;
using Jobs.Application.Abstractions.Messaging;
 
namespace Jobs.Application.Features.Companies.Commands.CreateCompany
{
	public record CreateCompanyCommand(
	string Name,
	string Address,
	string? description,
	string industry,
	string? logoUrl,

	// Address
	string Country,
	string City,
	string Street,
	string BuildingNumber,
	string PostalCode

	) : ICommand<Guid>;
}
