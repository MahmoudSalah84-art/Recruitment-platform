using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Common.Emails;
using Jobs.Domain.Events.Company_Events;
using Jobs.Domain.Events.Events;
using MediatR;

namespace Jobs.Application.Features.Identity.EventHandler.GenerateEmailConfirmationTokenAfterRegister
{
	public class GenerateEmailConfirmationTokenEventHandler : INotificationHandler<CompanyCreatedEvent> , INotificationHandler< UserRegisteredEvent>
	{
		private readonly IIdentityService _svc;
		private readonly IEmailSender _emailSender;
		public GenerateEmailConfirmationTokenEventHandler(IIdentityService svc, IEmailSender emailSender)
		{
			_svc = svc;
			_emailSender = emailSender;
		}

		public async Task Handle(CompanyCreatedEvent notification, CancellationToken cancellationToken)
		{
			//var result = await _svc.GetUserByIdAsync(notification.CompanyId);
			var result = await _svc.GetUserByIdAsync(notification.Id);
			if (result.IsFailure)
				return ;

			var tokenResult = await _svc.GenerateEmailConfirmationTokenAsync(notification.Id);
			var confirmUrl = $"http://jooobs.runasp.net/api/auth/confirm-email?userId={notification.Id}&token={Uri.EscapeDataString(tokenResult.Value)}";
			var message = new EmailConfirmationMessage(
				To: result.Value.Email,
				UserName: $"{result.Value.FirstName} {result.Value.LastName} {result.Value.UserName}",
				ConfirmationLink: confirmUrl);

			await _emailSender.SendAsync(message);

		}

		public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
		{
			//var result = await _svc.GetUserByIdAsync(notification.CompanyId);
			var result = await _svc.GetUserByIdAsync(notification.Id);
			if (result.IsFailure)
				return;

			var tokenResult = await _svc.GenerateEmailConfirmationTokenAsync(notification.Id);

			var confirmUrl = $"http://jooobs.runasp.net/api/auth/confirm-email?userId={notification.Id}&token={Uri.EscapeDataString(tokenResult.Value)}";
			var message = new EmailConfirmationMessage(
				To: result.Value.Email,
				UserName: $"{result.Value.FirstName} {result.Value.LastName} {result.Value.UserName}",
				ConfirmationLink: confirmUrl);

			await _emailSender.SendAsync(message);
		}
	}
}
