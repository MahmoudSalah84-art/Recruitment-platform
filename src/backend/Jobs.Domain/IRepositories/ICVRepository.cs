using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepositories
{
	public interface ICVRepository : IRepository<CV>
	{
		/// <summary>
		/// Retrieves a CV by its Id including the parsed details.
		/// </summary>
		/// <param name="id">The Id of the CV.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>The CV entity with parsed details if found, otherwise null.</returns>
		Task<CV?> GetByIdWithParsedAsync(string id, CancellationToken ct = default);
	}
}
