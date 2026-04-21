using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Identity.Command.Login
{
	public record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;
}
