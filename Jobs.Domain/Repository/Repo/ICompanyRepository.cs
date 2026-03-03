using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Repository.Repo
{
	public interface ICompanyRepository : IRepository<Company>
	{
		Task<Company?> GetByNameAsync(string name, CancellationToken ct = default);
	}
}
