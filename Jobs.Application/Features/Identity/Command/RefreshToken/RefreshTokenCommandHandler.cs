using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Identity.Command.RefreshToken
{
	public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthResponse>
	{
		private readonly IIdentityService _svc;
		public RefreshTokenCommandHandler(IIdentityService svc) => _svc = svc;

		public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand cmd, CancellationToken ct)
			=> await _svc.RefreshTokenAsync(cmd.AccessToken, cmd.RefreshToken);
	}
}
