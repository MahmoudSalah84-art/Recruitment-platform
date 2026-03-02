using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Identity.Command.ForgotPassword
{
	public record ForgotPasswordCommand(string Email) : ICommand<string>;

}
