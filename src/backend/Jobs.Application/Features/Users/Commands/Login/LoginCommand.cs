using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Users.Commands.Login
{
	public record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;
}
