using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories.IRepository
{
	public interface IRepository<TEntity> where TEntity : class
	{
		Task<TEntity?> GetByIdAsync(Guid id);
		void Add(TEntity entity);
		void Update(TEntity entity);
		void Remove(TEntity entity);
		IQueryable<TEntity> Query();
	}
}
