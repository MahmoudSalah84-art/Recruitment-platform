using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Identity.Command.ChangePassword
{
	public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
	{
		private readonly IIdentityService _svc;
		public ChangePasswordCommandHandler(IIdentityService svc) => _svc = svc;


		public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
		{
			return await _svc.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword);
		}
	}
}
