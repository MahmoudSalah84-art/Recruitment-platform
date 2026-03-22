using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Companies.Command.UpdateCompanyLogo
{
	public record UpdateCompanyLogoCommand(
	string CompanyId,
	FileUploadDto file) : ICommand;
}