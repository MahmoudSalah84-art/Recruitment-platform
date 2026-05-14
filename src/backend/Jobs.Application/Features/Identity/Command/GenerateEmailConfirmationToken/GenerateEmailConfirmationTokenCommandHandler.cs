using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Common.Emails;
using Jobs.Domain.Entities;

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

			var user = await _svc.GetUserByEmailAsync(request.Email);
			if (user.IsFailure )
				return Result.Success();
			
			var tokenResult = await _svc.GenerateEmailConfirmationTokenAsync(user.Value.Id);

			var confirmUrl = $"http://jooobs.runasp.net/api/auth/confirm-email?userId={user.Value.Id}&token={Uri.EscapeDataString(tokenResult.Value)}";
			var message = new EmailConfirmationMessage(
				To: user.Value.Email,
				UserName: $"{user.Value.FirstName} {user.Value.LastName} {user.Value.UserName}",
				ConfirmationLink: confirmUrl);

			await _emailSender.SendAsync(message );

			return Result.Success();
		}
	}
}