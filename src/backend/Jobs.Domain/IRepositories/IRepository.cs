using Jobs.Domain.Common;
using Jobs.Domain.Specifications;
using System.Linq.Expressions;


namespace Jobs.Domain.IRepositories
{
	public interface IRepository<TEntity> where TEntity : BaseEntity
	{
		// Retrieve by Id
		Task<TEntity?> GetByIdAsync(string id, CancellationToken ct = default);

		// Add entity
		void Add(TEntity entity);

		// Update entity
		void Update(TEntity entity);

		// Remove entity
		void Remove(TEntity entity);
		// Check if any entity matches the predicate
		Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

		// Queryable for custom queries
		IQueryable<TEntity> Query();

		Task AddRangeAsync(IEnumerable<TEntity> entities);


		// Specification pattern support
		Task<List<TEntity>> ListWithSpecAsync(ISpecification<TEntity> spec);
		Task<int> CountAsync(ISpecification<TEntity> spec);

	}
}
