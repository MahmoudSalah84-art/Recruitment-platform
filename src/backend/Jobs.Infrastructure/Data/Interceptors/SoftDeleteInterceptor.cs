using Jobs.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
			if (entry.Entity is SoftDelete sd) // check if entity implements SoftDelete
			{
				if (entry.State == EntityState.Deleted)
				{
					entry.State = EntityState.Modified; // convert delete to soft-delete
					sd.MarkAsDeleted(); // use public method to set IsDeleted/DeletedAt (fixes CS0272)
				}
			}
		}
		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}

