using Jobs.Application.Abstractions.Messaging;


namespace Jobs.Application.Features.Identity.Command.ChangePassword
{
	public record ChangePasswordCommand(
		string UserId,
		string CurrentPassword,
		string NewPassword
	) : ICommand;

}

