using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Identity.Command.ResetPassword
{
	public record ResetPasswordCommand(
		string Email,
		string Token,
		string NewPassword
	) : ICommand;
}

