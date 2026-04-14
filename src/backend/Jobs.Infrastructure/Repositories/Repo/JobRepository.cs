using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Infrastructure.Repositories.Repo
{
	public class JobRepository : Repository<Job>, IJobRepository
	{
		public JobRepository(JobDbContext context) : base(context) { }

		/// <summary>
		/// Retrieves a Job by its Id including the related Skills.
		/// </summary>
		/// <param name="id">The Id of the
		/// Job.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>The Job entity with Skills if found, otherwise null.</returns>
		public async Task<Job?> GetByIdWithSkillsAsync(string id, CancellationToken ct = default)
		{
			return await _set
				.AsNoTracking()            
				.Include(j => j.RequiredSkills) 
				.ThenInclude(js => js.Skill)
				.FirstOrDefaultAsync(j => j.Id == id, ct); 
		}

		/// <summary>
		/// Searches for Jobs that match the query string, with pagination.
		/// </summary>
		/// <param name="query">Search query to match Job title or description.</param>
		/// <param name="page">Page number (1-based).</param>
		/// <param name="pageSize">Number of items per page.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>List of Jobs matching the search query.</returns>
	

	}
}
