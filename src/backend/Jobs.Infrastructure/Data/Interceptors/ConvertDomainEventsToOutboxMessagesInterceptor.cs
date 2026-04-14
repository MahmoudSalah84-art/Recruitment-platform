using Jobs.Domain.Common;
using Jobs.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace Jobs.Infrastructure.Data.Interceptors
{
    public class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
	{
		public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
			DbContextEventData eventData,
			InterceptionResult<int> result,
			CancellationToken cancellationToken = default)
		{
			var dbContext = eventData.Context;
			if (dbContext == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

			var aggregates = dbContext.ChangeTracker
				.Entries<AggregateRoot>()
				.Select(x => x.Entity);
			var outboxMessages = aggregates
				.SelectMany(aggregateRoot => {
					var events = aggregateRoot.Events;
					//aggregateRoot.ClearEvents(); error
					return events;
				})
				// convert entity to OutboxMessage Entity
				.Select(domainEvent => new OutboxMessage
				(
					domainEvent.GetType().Name,
					JsonSerializer.Serialize(domainEvent)
				))
				.ToList();
			foreach (var agg in aggregates)	agg.ClearEvents();
				


				
			// 3. Add it for the same Transaction
			dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}
	}
}
