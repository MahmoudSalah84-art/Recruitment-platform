using Jobs.Domain.Entities;


namespace Jobs.Domain.IRepositories
{
	public interface IUserRepository : IRepository<User>
	{
		Task<User?> GetUserWithDetailsAsync(string userId, CancellationToken cancellationToken);
	} 
}
