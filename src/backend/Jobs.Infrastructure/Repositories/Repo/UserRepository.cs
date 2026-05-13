using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Infrastructure.Repositories.Repo
{
	public class UserRepository : Repository<User>, IUserRepository
	{
        public UserRepository(JobDbContext context) : base(context) { }


		public async Task<User?> GetUserWithDetailsAsync(string userId, CancellationToken cancellationToken)
		{
			return await _set
				.Include(u => u.CV)
				.Include(u => u.Applications)
				.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
		}

	}
}