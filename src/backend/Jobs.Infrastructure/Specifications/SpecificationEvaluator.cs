using Jobs.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Infrastructure.Specifications
{
	public class SpecificationEvaluator<TEntity> where TEntity : class
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
		{
			var query = inputQuery;

			if (spec.Criteria != null)
				query = query.Where(spec.Criteria);



			if(spec.Includes != null)
				//query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
				foreach (var include in spec.Includes)
				{
					query = query.Include(include);
				}



			if(spec.thenIncludes != null)
				foreach (var thenInclude in spec.thenIncludes)
				{
					query = query.Include(thenInclude);
				}
			

			if (spec.OrderBy != null)
				query = query.OrderBy(spec.OrderBy);
			else if (spec.OrderByDescending != null)
				query = query.OrderByDescending(spec.OrderByDescending);

			return query;
		}
	}
}
