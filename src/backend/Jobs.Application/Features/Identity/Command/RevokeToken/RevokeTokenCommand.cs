using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Identity.Command.RevokeToken
{
	public record RevokeTokenCommand(string RefreshToken) : ICommand;
}
