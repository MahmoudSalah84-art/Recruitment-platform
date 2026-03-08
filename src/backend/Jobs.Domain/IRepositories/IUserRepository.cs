using Jobs.Domain.Entities;


namespace Jobs.Domain.IRepositories
{
	public interface IUserRepository : IRepository<User>
	{
		/// <summary>
		/// Retrieves a user by email if exists.
		/// </summary>
		/// <param name="email">The email of the user.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>The user entity if found, otherwise null.</returns>
		Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

		/// <summary>
		/// Checks if a user with the specified email exists in the database.
		/// </summary>
		/// <param name="email">The email to check for existence.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>True if a user with the given email exists, otherwise false.</returns>
		Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
	}
}
