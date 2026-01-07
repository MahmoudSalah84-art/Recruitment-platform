using Jobs.Domain.Entities;
using Jobs.Domain.IRepository.IRepo;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories.Repo
{
	public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
	{
		public CompanyRepository(JobDbContext context) : base(context)
		{
		}

        public Task AddAsync(Company company, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Company?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Company?> GetByNameAsync(string name)
		{
			return await _set.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name);
		}

        public Task UpdateAsync(Company company, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
