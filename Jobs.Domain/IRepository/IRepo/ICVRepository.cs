using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepository.IRepo
{
	public interface ICVRepository : IRepository<CV>
	{
		Task<CV?> GetByIdWithParsedAsync(Guid id);

		Task<CV?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(CV cv, CancellationToken ct = default);
		Task UpdateAsync(CV cv, CancellationToken ct = default);
	}
}
