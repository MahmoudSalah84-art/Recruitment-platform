using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Identity.Command.ForgotPassword
{
	public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand,string>
	{
		private readonly IIdentityService _svc;
		public ForgotPasswordCommandHandler(IIdentityService svc) => _svc = svc;


		public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
		{
			var token = await _svc.GeneratePasswordResetTokenAsync(request.Email);

			return Result<string>.Success(token);
		}
	}
}
