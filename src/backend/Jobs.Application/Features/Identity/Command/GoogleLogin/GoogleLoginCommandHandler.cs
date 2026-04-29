using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Identity.Command.GoogleLogin
{
	public sealed class GoogleLoginCommandHandler : ICommandHandler<GoogleLoginCommand, AuthResponse>
	{
		private readonly IIdentityService _svc;
		
		public GoogleLoginCommandHandler(
			IIdentityService svc)
		{
			_svc = svc;
		}

		public async Task<Result<AuthResponse>> Handle( GoogleLoginCommand request, CancellationToken cancellationToken)
		{
			return await _svc.LoginWithGoogleAsync(request.IdToken);
		}
	}
}
