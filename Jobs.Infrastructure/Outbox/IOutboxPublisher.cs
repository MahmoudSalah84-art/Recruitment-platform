using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Outbox
{
	public interface IOutboxPublisher
	{
		Task PublishAsync(OutboxMessage message, CancellationToken ct = default);
	}
}
