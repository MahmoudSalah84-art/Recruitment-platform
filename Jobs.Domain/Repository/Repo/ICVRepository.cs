using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Repository.Repo
{
	public interface ICVRepository : IRepository<CV>
	{
		Task<CV?> GetByIdWithParsedAsync(Guid id, CancellationToken ct = default);
	}
}
