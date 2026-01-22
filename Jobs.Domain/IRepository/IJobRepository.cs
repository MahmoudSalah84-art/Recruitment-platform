using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepository
{
	public interface IJobRepository : IRepository<Job>
	{
		Task<IEnumerable<Job>> SearchAsync(string query, int page = 1, int pageSize = 10, CancellationToken ct = default);
	}
}
