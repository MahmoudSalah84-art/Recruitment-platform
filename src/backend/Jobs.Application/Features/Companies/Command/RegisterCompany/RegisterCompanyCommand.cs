using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Companies.Command.Register
{
	public record RegisterCompanyCommand(
		string UserName,
		string Email,
		string Industry,
		string Country,
		string City,
		string Street,
		string BuildingNumber,
		string PostalCode,
		string? Description,
		FileUploadDto? Image,
		string Password,
		string ConfirmPassword
	) : ICommand<AuthResponse>;
}
