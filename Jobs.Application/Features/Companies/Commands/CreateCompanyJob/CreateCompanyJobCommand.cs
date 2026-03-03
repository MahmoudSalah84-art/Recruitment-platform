using Jobs.Application.Abstractions.Messaging;
using MediatR;

namespace Jobs.Application.Features.Companies.Commands.CreateCompanyJob
{
	public record CreateCompanyJobCommand(
	Guid CompanyId,
	string Title,
	string Description
	) : IRequest<Result<Guid>>;
}
