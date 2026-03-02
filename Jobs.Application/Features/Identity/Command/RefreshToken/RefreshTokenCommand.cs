using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Identity.Command.RefreshToken
{
	public record RefreshTokenCommand(string AccessToken, string RefreshToken) : ICommand<AuthResponse>;

}
