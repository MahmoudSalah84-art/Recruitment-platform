using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Common.Emails;
using Jobs.Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;

namespace Jobs.Infrastructure.Services
{
	public class SmtpEmailSender : IEmailSender
	{
		private readonly EmailSettings _settings;
		private readonly ILogger<SmtpEmailSender> _logger;

		public SmtpEmailSender(IOptions<EmailSettings> settings, ILogger<SmtpEmailSender> logger)
		{
			_settings = settings.Value;
			_logger = logger;
		}

		public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
		{
			try
			{
				using var client = new SmtpClient();

				await client.ConnectAsync(
					_settings.Host,
					_settings.Port,
					_settings.EnableSsl
						? MailKit.Security.SecureSocketOptions.SslOnConnect
						: MailKit.Security.SecureSocketOptions.StartTls,
					cancellationToken);

				await client.AuthenticateAsync(
					_settings.SenderEmail,
					_settings.Password,
					cancellationToken);

				var email = new MimeMessage();
				email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
				email.To.Add(MailboxAddress.Parse(message.To));
				email.Subject = message.Subject;
				email.Body = new TextPart(TextFormat.Html) { Text = message.BuildBody() };

				await client.SendAsync(email, cancellationToken);
				await client.DisconnectAsync(true, cancellationToken);

				_logger.LogInformation(
					"Email sent to {To} with subject {Subject}", message.To, message.Subject);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex,
					"Failed to send email to {To} with subject {Subject}", message.To, message.Subject);

				throw new EmailSendException($"Failed to send email to {message.To}.", ex);
			}
		}
	}

}