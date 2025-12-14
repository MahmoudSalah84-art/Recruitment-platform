using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Outbox
{
	public class OutboxMessage
	{
		public Guid Id { get; private set; } = Guid.NewGuid();
		public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
		public string Type { get; private set; } = string.Empty;
		public string Content { get; private set; } = string.Empty;
		public bool Processed { get; private set; } = false;
		public DateTime? ProcessedOn { get; private set; }

		public OutboxMessage() { }

		public OutboxMessage(string type, string content)
		{
			Type = type;
			Content = content;
		}

		public void MarkProcessed()
		{
			Processed = true;
			ProcessedOn = DateTime.UtcNow;
		}
	}
}
