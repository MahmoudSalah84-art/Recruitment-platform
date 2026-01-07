using Jobs.Domain.Entities;
using Jobs.Domain.Interfaces;
using Jobs.Domain.IRepository.IRepo;
using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories.Repo
{
	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		public UserRepository(JobDbContext context) : base(context)
		{
		}

        public Task AddAsync(User user, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EmailExistsAsync(string email)
		{
			if (string.IsNullOrWhiteSpace(email)) return false;
			return await _set
				.AsNoTracking()
				.Where(u => EF.Property<string>(u, "Email") == email)
				.AnyAsync();
		}

        public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByIdentityIdAsync(Guid identityUserId)
		{
			return await _set
				.AsNoTracking()
				.Where(u => EF.Property<Guid?>(u, "IdentityUserId") == identityUserId)
				.FirstOrDefaultAsync();
		}

        public Task UpdateAsync(User user, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
