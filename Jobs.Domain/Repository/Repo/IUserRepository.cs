using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Repository.Repo
{
	public interface IUserRepository : IRepository<User>
	{
		Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
		Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
	}
}
