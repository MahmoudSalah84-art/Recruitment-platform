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

		public async Task<IEnumerable<Job>> GetAllActiveJobsAsync(CancellationToken ct = default)
		{
			return await _set
				.AsNoTracking()
				.Where(j => j.IsPublished && (j.ExpirationDate == null || j.ExpirationDate > DateTime.UtcNow))
				.ToListAsync(ct);
		}
	}
}
