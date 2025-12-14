using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Services
{
	public interface IEmailService
	{
		Task SendAsync(string to, string subject, string htmlBody);
	}

	/// <summary>
	/// Minimal implementation that can be replaced by SMTP / SendGrid / Amazon SES etc.
	/// </summary>
	public class EmailService : IEmailService
	{
		private readonly EmailServiceOptions _options;

		public EmailService(EmailServiceOptions options)
		{
			_options = options;
		}

		public Task SendAsync(string to, string subject, string htmlBody)
		{
			// Production: use SMTP client or third-party SDK.
			// For now, just do an async no-op or logging.
			// Example: SmtpClient.SendMailAsync(...)
			return Task.CompletedTask;
		}
	}

	public class EmailServiceOptions
	{
		public string From { get; set; } = string.Empty;
		public string SmtpHost { get; set; } = string.Empty;
		public int SmtpPort { get; set; } = 587;
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public bool UseSsl { get; set; } = true;
	}
}
