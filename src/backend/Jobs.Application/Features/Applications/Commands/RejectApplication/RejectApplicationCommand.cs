using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Applications.Commands.RejectApplication
{
	public record RejectApplicationCommand(
	string ApplicationId,
	string? Note) : ICommand;
}
