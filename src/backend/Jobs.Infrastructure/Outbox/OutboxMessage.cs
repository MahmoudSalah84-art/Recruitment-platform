using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jobs.Infrastructure.Outbox
{
	public class OutboxMessage
	{
		public Guid Id { get; private set; } = Guid.NewGuid();
		public DateTime OccurredOn { get; private set; } 
		public string Type { get; private set; } = string.Empty; // event name
		public string Content { get; private set; } = string.Empty; // Jason
		public bool Processed { get; private set; }
		public DateTime? ProcessedOn { get; private set; }
		public string? Error { get; private set; }

		public OutboxMessage() { }

		public OutboxMessage(string type, string content )
		{
			Type = type;
			Content = content;
			OccurredOn = DateTime.UtcNow;
			Processed = false;
		}

		public void MarkProcessed()
		{
			Processed = true;
			ProcessedOn = DateTime.UtcNow;
		}

		public void SetError(string errorMessage)
		{
			Error = errorMessage;
		}
	}
}
