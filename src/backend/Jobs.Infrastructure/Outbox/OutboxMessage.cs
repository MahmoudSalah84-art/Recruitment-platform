
namespace Jobs.Infrastructure.Outbox
{
	public class OutboxMessage
	{
		public string Id { get; private set; } = Guid.NewGuid().ToString();
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
