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
		/// Retrieves all active Jobs.
		/// </summary>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>List of active Jobs.</returns>
		Task<IEnumerable<Job>> GetAllActiveJobsAsync(CancellationToken ct = default);
	}
}
