using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Identity.Command.Login
{
	public class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResponse>
	{
		private readonly IIdentityService _svc;
		public LoginCommandHandler(IIdentityService svc) => _svc = svc;

		public async Task<Result<AuthResponse>> Handle(LoginCommand cmd, CancellationToken ct)
			=> await _svc.LoginAsync(new LoginRequest(cmd.Email, cmd.Password));
	}
}
