using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Interfaces
{
	public interface ICompanyRepository
	{
		Task<Company?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(Company company, CancellationToken ct = default);
		Task UpdateAsync(Company company, CancellationToken ct = default);
	}
}
