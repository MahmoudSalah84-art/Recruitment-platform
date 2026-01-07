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
	public class JobRepository : BaseRepository<Job>, IJobRepository
	{
		public JobRepository(JobDbContext context) : base(context)
		{
		}

        public Task AddAsync(Job job, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Job?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Job?> GetByIdWithSkillsAsync(Guid id)
		{
			return await _set
				.Include(j => j.RequiredSkills)
				.FirstOrDefaultAsync(j => j.Id == id);
		}

		public async Task<IEnumerable<Job>> SearchAsync(string query, int page = 1, int pageSize = 20)
		{
			var q = _set.AsQueryable();

			if (!string.IsNullOrWhiteSpace(query))
			{
				q = q.Where(j => EF.Functions.Like(j.Title, $"%{query}%") ||
								 EF.Functions.Like(j.Description, $"%{query}%"));
			}

			return await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
		}

        public Task<IEnumerable<Job>> SearchAsync(string query, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Job job, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
