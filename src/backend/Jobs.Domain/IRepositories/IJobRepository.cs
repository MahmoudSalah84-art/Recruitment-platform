using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepositories
{
	public interface IJobRepository : IRepository<Job>
	{
		/// <summary>
		/// Retrieves a Job by its Id including the related Skills.
		/// </summary>
		/// <param name="id">The Id of the Job.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>The Job entity with Skills if found, otherwise null.</returns>
		Task<Job?> GetByIdWithSkillsAsync(string id, CancellationToken ct = default);

		/// <summary>
		/// Searches for Jobs that match the query string, with pagination.
		/// </summary>
		/// <param name="Search">Search query to match Job title or description.</param>
		/// <param name="page">Page number (1-based).</param>
		/// <param name="pageSize">Number of items per page.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>List of Jobs matching the search query.</returns>
	}
}
