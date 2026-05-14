using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Identity.Command.GenerateEmailConfirmationToken
{
	public record GenerateEmailConfirmationTokenCommand(string Email) : ICommand;
}
