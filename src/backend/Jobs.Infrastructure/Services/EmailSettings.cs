using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Services
{
	public class EmailSettings
	{
		public string Host { get; init; } = string.Empty;
		public int Port { get; init; }
		public string SenderEmail { get; init; } = string.Empty;
		public string SenderName { get; init; } = string.Empty;
		public string Password { get; init; } = string.Empty;
		public bool EnableSsl { get; init; }
	}
}
