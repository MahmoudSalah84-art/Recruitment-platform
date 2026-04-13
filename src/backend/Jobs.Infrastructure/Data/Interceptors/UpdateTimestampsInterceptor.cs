using Jobs.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Jobs.Infrastructure.Data.Interceptors
{
	public class UpdateTimestampsInterceptor : SaveChangesInterceptor
	{

		public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
		{
			var dbContext = eventData.Context;
			if (dbContext == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

			var entries = dbContext.ChangeTracker.Entries()
				.Where(e => e.Entity is not OutboxMessage && (e.State == EntityState.Added || e.State == EntityState.Modified));
			

			foreach (var entry in entries)
			{
				var propCreated = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "CreatedAt");
				var propUpdated = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "UpdatedAt");

				if (entry.State == EntityState.Added)
				{
					propCreated?.CurrentValue = DateTime.Now;
				}
				propUpdated?.CurrentValue = DateTime.Now;
			}
			
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}


		
	}
}
