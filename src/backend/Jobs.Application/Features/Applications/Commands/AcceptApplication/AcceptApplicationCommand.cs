
using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Applications.Commands.AcceptApplication
{
	public record AcceptApplicationCommand(
	string ApplicationId) : ICommand;
}
