using Jobs.Application.Common.Emails;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Jobs.Application.Abstractions.Interfaces
{

	public interface IEmailSender
	{
		Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
	}
}
