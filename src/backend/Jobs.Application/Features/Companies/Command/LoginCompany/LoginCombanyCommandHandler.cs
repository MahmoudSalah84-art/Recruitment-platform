using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Companies.Command.LoginCompany
{
	public class LoginCombanyCommandHandler : ICommandHandler<LoginCombanyCommand, AuthResponse>
	{
		private readonly IIdentityService _svc;
		public LoginCombanyCommandHandler(IIdentityService svc) => _svc = svc;

		public async Task<Result<AuthResponse>> Handle(LoginCombanyCommand cmd, CancellationToken ct)
			=> await _svc.LoginAsync(new LoginRequest(cmd.Email, cmd.Password));

	}
}
