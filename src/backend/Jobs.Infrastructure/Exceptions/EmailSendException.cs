using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Exceptions
{
	public class EmailSendException : Exception
	{
		public EmailSendException(string message, Exception inner) : base(message, inner) { }
	}
}
