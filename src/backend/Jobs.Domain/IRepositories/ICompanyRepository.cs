using Jobs.Domain.Entities;

namespace Jobs.Domain.IRepositories
{
	public interface ICompanyRepository : IRepository<Company>
	{
		/// <summary>
		/// Retrieves a Company by its name.
		/// </summary>
		/// <param name="name">The name of the company.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>The Company entity if found, otherwise null.</returns>
		Task<Company?> GetByNameAsync(string name, CancellationToken ct = default);

		Task<Company?> GetByIdWithEmployeeAsync(string companyId, CancellationToken cancellationToken);
		
	}
}
