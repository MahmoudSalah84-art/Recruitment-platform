using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
 

namespace Jobs.Application.Features.Identity.Command.RevokeToken
{
	public class RevokeTokenCommandHandler : ICommandHandler<RevokeTokenCommand>
	{
		private readonly IIdentityService _svc;
		public RevokeTokenCommandHandler(IIdentityService svc) => _svc = svc;

		public async Task<Result> Handle(RevokeTokenCommand cmd, CancellationToken ct)
			=> await _svc.RevokeRefreshTokenAsync(cmd.RefreshToken);
	}
}