using Jobs.Domain.Common;
using Jobs.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories.IRepository
{
	public interface IRepository<TEntity> where TEntity : BaseEntity
	{
		Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
		void Add(TEntity entity);
		Task AddAsync(TEntity entity, CancellationToken ct = default);
		void Update(TEntity entity);
		void Remove(TEntity entity);
		IQueryable<TEntity> Query();
		Task<List<TEntity>> ListWithSpecAsync(ISpecification<TEntity> spec);
		Task<int> CountAsync(ISpecification<TEntity> spec);

	}
}
