using Jobs.Domain.Common;
using Jobs.Domain.Common.YourProject.Domain.Common;
using Jobs.Infrastructure.Data;
using Jobs.Infrastructure.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> 
		where TEntity : AggregateRoot
	{
		protected readonly DbSet<TEntity> _set;
		protected BaseRepository(JobDbContext context)
		{
			_set = context.Set<TEntity>();
		}



		public virtual async Task<TEntity?> GetByIdAsync(Guid id)
		{
			return await _set.FindAsync(id);
			//return await _set.FirstOrDefaultAsync(x => x.Id == id);
		}

		public virtual void Add(TEntity entity) => _set.Add(entity);

		public virtual void Update(TEntity entity) => _set.Update(entity);

		public virtual void Remove(TEntity entity) => _set.Remove(entity);

		public virtual IQueryable<TEntity> Query() => _set.AsQueryable();




        //Task<TEntity?> IRepository<TEntity>.GetByIdAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}
        //IQueryable<TEntity> IRepository<TEntity>.Query()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
