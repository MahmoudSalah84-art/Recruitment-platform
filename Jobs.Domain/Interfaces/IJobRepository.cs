using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Interfaces
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Job job, CancellationToken ct = default);
        Task UpdateAsync(Job job, CancellationToken ct = default);
		Task<IEnumerable<Job>> SearchAsync(string query, CancellationToken ct = default);
	}
	
}
