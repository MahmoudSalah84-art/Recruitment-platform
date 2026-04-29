using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Identity.Command.GoogleLogin
{
	// Application/Auth/Commands/GoogleLogin/GoogleLoginCommand.cs
	public sealed record GoogleLoginCommand(string IdToken)
		: ICommand<AuthResponse>;
}
