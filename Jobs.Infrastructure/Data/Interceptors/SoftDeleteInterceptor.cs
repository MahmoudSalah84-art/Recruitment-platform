using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
	// This assumes entities implement ISoftDelete (IsDeleted property)
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		var ctx = eventData.Context;
		if (ctx == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

		foreach (var entry in ctx.ChangeTracker.Entries())
		{
			if (entry.Entity is ISoftDelete sd)
			{
				if (entry.State == EntityState.Deleted)
				{
					// convert delete to soft-delete
					entry.State = EntityState.Modified;
					sd.IsDeleted = true;
				}
			}
		}

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}

public interface ISoftDelete
{
	bool IsDeleted { get; set; }
}