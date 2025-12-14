using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Interfaces
{
	public interface ICVRepository
	{
		Task<CV?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(CV cv, CancellationToken ct = default);
		Task UpdateAsync(CV cv, CancellationToken ct = default);
	}
}
