using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepository.IRepo
{
	public interface IJobRepository : IRepository<Job>
	{
		Task<Job?> GetByIdWithSkillsAsync(Guid id);
		Task<IEnumerable<Job>> SearchAsync(string query, int page = 1, int pageSize = 20);

		Task<Job?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(Job job, CancellationToken ct = default);
		Task UpdateAsync(Job job, CancellationToken ct = default);
		Task<IEnumerable<Job>> SearchAsync(string query, CancellationToken ct = default);
	}
}
