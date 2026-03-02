using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Identity.Command.Register
{
	public record RegisterCommand(
	string FirstName,
	string LastName,
	string Email,
	string Password,
	string ConfirmPassword
	) : ICommand<AuthResponse>;

}
