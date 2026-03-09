using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Jobs.Commands.UnpublishJob
{
	public record UnpublishJobCommand(string JobId) : ICommand;
}
