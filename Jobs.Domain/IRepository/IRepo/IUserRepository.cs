using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.IRepository.IRepo
{
	public interface IUserRepository : IRepository<User>
	{
		Task<bool> EmailExistsAsync(string email);

		Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
		Task AddAsync(User user, CancellationToken ct = default);
		Task UpdateAsync(User user, CancellationToken ct = default);

	}
}
