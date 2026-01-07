using Jobs.Domain.Entities;
using Jobs.Domain.Interfaces;
using Jobs.Domain.IRepository.IRepo;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories.Repo
{
	public class CVRepository : BaseRepository<CV>, ICVRepository
	{
		public CVRepository(JobDbContext context) : base(context)
		{
		}

        public Task AddAsync(CV cv, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<CV?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<CV?> GetByIdWithParsedAsync(Guid id)
		{
			return await _set.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
		}

        public Task UpdateAsync(CV cv, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
