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
		/// <param name="id">The Id of the Job.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>The Job entity with Skills if found, otherwise null.</returns>
		public async Task<Job?> GetByIdWithSkillsAsync(string id, CancellationToken ct = default)
		{
			return await _set
				.AsNoTracking()            
				.Include(j => j.RequiredSkills)              
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
		public async Task<IEnumerable<Job>> SearchAsync(string Search, int page = 1,int pageSize = 10,CancellationToken ct = default)
		{
			if (page <= 0) page = 1;
			if (pageSize <= 0) pageSize = 10;

			var jobsQuery = _set
				.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(Search))
			{
				var normalizedSearch = Search.Trim().ToLower();
				jobsQuery = jobsQuery.Where(j =>
					j.Title.ToLower().Contains(normalizedSearch) ||
					j.Description.ToLower().Contains(normalizedSearch));
			}

			jobsQuery = jobsQuery
				.OrderBy(j => j.Title)
				.Skip((page - 1) * pageSize)
				.Take(pageSize);

			return await jobsQuery.ToListAsync(ct);
		}

    }
}
