using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepository.IRepo
{
	public interface ICompanyRepository : IRepository<Company>
	{
		Task<Company?> GetByNameAsync(string name);

		Task<Company?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(Company company, CancellationToken ct = default);
		Task UpdateAsync(Company company, CancellationToken ct = default);
	}
}
