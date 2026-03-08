using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Infrastructure.Repositories.Repo
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(JobDbContext context) : base(context) { }


		/// <summary>
		/// Retrieves a Company by its name.
		/// </summary>
		/// <param name="name">The name of the company.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>The Company entity if found, otherwise null.</returns>
		public async Task<Company?> GetByNameAsync(string name, CancellationToken ct = default)
		{
			return await _set
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Name == name, ct);
		}

	}
}
