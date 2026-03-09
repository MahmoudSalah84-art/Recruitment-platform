using Jobs.Application.Abstractions.Messaging;


namespace Jobs.Application.Features.Jobs.Commands.PublishJob
{

	public record PublishJobCommand(string JobId) : ICommand;

}
