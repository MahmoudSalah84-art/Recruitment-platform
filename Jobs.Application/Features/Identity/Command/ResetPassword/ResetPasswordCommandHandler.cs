using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;

namespace Jobs.Application.Features.Identity.Command.ResetPassword
{
	public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
	{
		private readonly IIdentityService _svc;
		public ResetPasswordCommandHandler(IIdentityService svc) => _svc = svc;


		public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
		{
			return await _svc.ResetPasswordAsync( new ResetPasswordRequest(request.Email, request.Token,request.NewPassword));
		}
	}
}
