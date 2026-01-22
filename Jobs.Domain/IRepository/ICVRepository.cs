using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepository
{
	public interface ICVRepository : IRepository<CV>
	{
		Task<CV?> GetByIdWithParsedAsync(Guid id, CancellationToken ct = default);
	}
}
