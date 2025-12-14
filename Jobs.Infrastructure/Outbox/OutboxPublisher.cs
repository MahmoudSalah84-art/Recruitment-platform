using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Outbox
{
	/// <summary>
	/// Skeleton implementation: publish to message broker (RabbitMQ / Kafka / Azure Service Bus).
	/// Real implementation should handle retries, DLQ, serialization, etc.
	/// </summary>
	public class OutboxPublisher : IOutboxPublisher
	{
		public Task PublishAsync(OutboxMessage message, CancellationToken ct = default)
		{
			// Example: publish to generic message broker.
			// For now this is a no-op placeholder.
			return Task.CompletedTask;
		}
	}
}
