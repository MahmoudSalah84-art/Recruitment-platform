using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Emails;
using Jobs.Domain.ValueObjects;
using System.Text;

namespace Jobs.Application.Features.Identity.Command.ForgotPassword
{
	public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
	{
		private readonly IIdentityService _svc;
		private readonly IEmailSender _emailSender;

		public ForgotPasswordCommandHandler(IIdentityService svc, IEmailSender emailSender) => (_svc,_emailSender) = (svc,emailSender);
		

		public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
		{
			var token = await _svc.GeneratePasswordResetTokenAsync(request.Email);
			if (token is null)
				return Result.Success();

			//await _emailService.SendPasswordResetEmailAsync(user.Email!, user.UserName!, encodedToken);

			var ResetLink = $"http://jooobs.runasp.net/api/Auth/reset-password?token={Uri.EscapeDataString(token)}&email={request.Email}";
			var message = new PasswordResetMessage(
				To: request.Email,
				UserName: " $$user$$ ",
				ResetLink: ResetLink
			);

			await _emailSender.SendAsync(message);

			return Result.Success();
		}
	}
}
