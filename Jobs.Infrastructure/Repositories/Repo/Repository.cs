using Jobs.Domain.Common;
using Jobs.Domain.Specifications;
using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Repositories.IRepository;
using Jobs.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Infrastructure.Repositories.Repo
{
    public abstract class Repository<TEntity> : IRepository<TEntity> 
		where TEntity : AggregateRoot
	{
		protected readonly DbSet<TEntity> _set;
		protected Repository(JobDbContext context)=> _set = context.Set<TEntity>();


		// is more efficient than FirstOrDefaultAsync because it checks the context first (in-memory) before querying the database
		/// <summary>
		/// Retrieves an entity by its unique identifier.
		/// </summary>
		/// <param name="id">The unique identifier of the entity.</param>
		/// <param name="ct">Cancellation token to cancel the operation.</param>
		/// <returns>The entity if found; otherwise, null.</returns>
		public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
			=> await _set.FindAsync(id , ct);

		/// <summary>
		/// Adds a new entity to the database context.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public void Add(TEntity entity)
			=> _set.Add(entity);

		/// <summary>
		/// Updates an existing entity in the database context.
		/// </summary>
		/// <param name="entity">The entity to update.</param>
		public void Update(TEntity entity)
			 => _set.Update(entity);

		/// <summary>
		/// Removes an entity from the database context.
		/// </summary>
		/// <param name="entity">The entity to remove.</param>
		public void Remove(TEntity entity)
			=> _set.Remove(entity);

		/// <summary>
		/// Returns a queryable for the entity set, allowing further filtering or projection.
		/// </summary>
		/// <returns>An IQueryable of TEntity.</returns>
		public virtual IQueryable<TEntity> Query() 
			=> _set.AsNoTracking().AsQueryable();



		public async Task<List<TEntity>> ListWithSpecAsync(ISpecification<TEntity> spec)
			=> await SpecificationEvaluator<TEntity>.GetQuery(Query(), spec).ToListAsync();
		

		public async Task<int> CountAsync(ISpecification<TEntity> spec)
			=> await SpecificationEvaluator<TEntity>.GetQuery(Query(), spec).CountAsync();
		
	}
}
