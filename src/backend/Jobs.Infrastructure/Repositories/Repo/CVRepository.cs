using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories.Repo
{
	public class CVRepository : Repository<CV>, ICVRepository
	{
		public CVRepository(JobDbContext context) : base(context) { }

		/// <summary>
		/// Retrieves a CV by its Id including the parsed details.
		/// </summary>
		/// <param name="id">The Id of the CV.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>The CV entity with parsed details if found, otherwise null.</returns>
		public async Task<CV?> GetByIdWithParsedAsync(string id, CancellationToken ct = default)
		{
			return await _set
				.AsNoTracking()
				.Include(cv => cv.ParsedData)
				.FirstOrDefaultAsync(cv => cv.Id == id, ct);
		}

    }
}
