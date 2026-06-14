using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Enums;

namespace Jobs.Application.Features.Applications.Commands.UpdateApplicationStatus
{

	public record UpdateApplicationStatusCommand(
	string CompanyId,
	string ApplicationId,
	ApplicationStatus NewStatus
) : ICommand;
}
