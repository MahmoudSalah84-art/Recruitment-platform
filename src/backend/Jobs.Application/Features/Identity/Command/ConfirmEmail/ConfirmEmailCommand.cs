using Jobs.Application.Abstractions.Messaging;


namespace Jobs.Application.Features.Identity.Command.ConfirmEmail
{
	public record ConfirmEmailCommand(string UserId, string Token) : ICommand;

}
