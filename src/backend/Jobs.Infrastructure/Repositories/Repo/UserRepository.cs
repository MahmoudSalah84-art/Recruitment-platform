using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Infrastructure.Repositories.Repo
{
	public class UserRepository : Repository<User>, IUserRepository
	{
        public UserRepository(JobDbContext context) : base(context) { }


		/// <summary>
		/// Retrieves a user by email if exists.
		/// </summary>
		/// <param name="email">The email of the user.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>The user entity if found, otherwise null.</returns>
		public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
		{
			if(string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("Email must be provided.", nameof(email));

			return _set
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Email.Value == email, ct);
		}


		/// <summary>
		/// Checks if a user with the specified email exists in the database.
		/// </summary>
		/// <param name="email">The email to check for existence.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>True if a user with the given email exists, otherwise false.</returns>
		public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("Email must be provided.", nameof(email));

			return await _set
				.AsNoTracking()
				.AnyAsync(u => u.Email.Value == email, ct);
		}

		
	}
}