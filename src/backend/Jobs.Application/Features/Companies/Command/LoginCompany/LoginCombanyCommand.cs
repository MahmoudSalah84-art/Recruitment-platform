using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Companies.Command.LoginCompany
{
	public record LoginCombanyCommand(string Email, string Password) : ICommand<AuthResponse>;
}
