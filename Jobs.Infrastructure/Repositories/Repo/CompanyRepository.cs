using Jobs.Domain.Entities;
using Jobs.Domain.IRepository.IRepo;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories.Repo
{
	public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
	{
		public CompanyRepository(JobDbContext context) : base(context)
		{
		}

		public async Task<Company?> GetByNameAsync(string name)
		{
			return await _set.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name);
		}
	}
}
