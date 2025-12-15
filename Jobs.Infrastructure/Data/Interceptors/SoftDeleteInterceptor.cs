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
		var ctx = eventData.Context;
		if (ctx == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

		foreach (var entry in ctx.ChangeTracker.Entries())
		{
			if (entry.Entity is ISoftDelete sd) // check if entity implements ISoftDelete
			{
				if (entry.State == EntityState.Deleted)
				{
					entry.State = EntityState.Modified; // convert delete to soft-delete
					sd.IsDeleted = true;// UPDATE Users SET IsDeleted = 1 WHERE Id = 1
				}
			}
		}
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}

