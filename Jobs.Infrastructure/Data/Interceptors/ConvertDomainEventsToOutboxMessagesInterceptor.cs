using Jobs.Domain.Common;
using Jobs.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace Jobs.Infrastructure.Data.Interceptors
{
    internal class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
	{
		public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
			DbContextEventData eventData,
			InterceptionResult<int> result,
			CancellationToken cancellationToken = default)
		{
			var dbContext = eventData.Context;
			if (dbContext == null) return base.SavingChangesAsync(eventData, result, cancellationToken);


			var outboxMessages = dbContext.ChangeTracker
				.Entries<AggregateRoot>()
				.Select(x => x.Entity)
				.SelectMany(aggregateRoot => {
					var events = aggregateRoot.Events;
					aggregateRoot.ClearEvents();
					return events;
				})
				// convert entity to OutboxMessage Entity
				.Select(domainEvent => new OutboxMessage
				(
					domainEvent.GetType().Name,
					JsonSerializer.Serialize(domainEvent)
				)).ToList();

			// 3. إضافتها لنفس الـ Context ليتم حفظها في نفس الـ Transaction
			dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}
	}
}
