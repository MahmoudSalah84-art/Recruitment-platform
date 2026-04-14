using Jobs.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		var dbContext = eventData.Context;
		if (dbContext == null) return base.SavingChangesAsync(eventData, result, cancellationToken);
		
		foreach (var entry in dbContext.ChangeTracker.Entries())
		{
			if (entry.Entity is SoftDelete sd) // check if entity implements SoftDelete
			{
				if (entry.State == EntityState.Deleted)
				{
					entry.State = EntityState.Modified; // convert delete to soft-delete
					sd.MarkAsDeleted(); // use public method to set IsDeleted/DeletedAt
				}
			}
		}
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}

