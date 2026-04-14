using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Users.Commands.Register
{
	public record RegisterCommand(
	string FirstName,
	string LastName,
	string Email,
	string PhoneNumber,
	string Bio,
	string Password,
	string ConfirmPassword
	) : ICommand<AuthResponse>;

}
