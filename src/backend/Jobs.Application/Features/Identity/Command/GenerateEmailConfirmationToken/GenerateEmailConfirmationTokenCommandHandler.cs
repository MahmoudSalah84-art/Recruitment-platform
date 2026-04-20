using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Common.Emails;

namespace Jobs.Application.Features.Identity.Command.GenerateEmailConfirmationToken
{
	public class GenerateEmailConfirmationTokenCommandHandler : ICommandHandler<GenerateEmailConfirmationTokenCommand>
	{
		private readonly IIdentityService _svc;
		private readonly IEmailSender _emailSender;
		public GenerateEmailConfirmationTokenCommandHandler(IIdentityService svc, IEmailSender emailSender) 
		{
			_svc = svc;
			_emailSender = emailSender;
		}


		public async Task<Result> Handle(GenerateEmailConfirmationTokenCommand request, CancellationToken cancellationToken)
		{
			var result =await _svc.GetUserByIdAsync(request.UserId);
			if (result.IsFailure)
				return Result.Failure(result.Error);

			
			var tokenResult = await _svc.GenerateEmailConfirmationTokenAsync(request.UserId);

			var confirmUrl = $"http://jooobs.runasp.net/api/auth/confirm-email?userId={request.UserId}&token={Uri.EscapeDataString(tokenResult.Value)}";
			var message = new EmailConfirmationMessage(
				To: result.Value.Email,
				UserName: $"{result.Value.FirstName} {result.Value.LastName} {result.Value.UserName}",
				ConfirmationLink: confirmUrl);

			await _emailSender.SendAsync(message );

			return Result.Success();
		}
	}
}