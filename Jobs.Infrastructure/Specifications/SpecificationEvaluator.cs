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

			foreach (var include in spec.Includes)
			{
				query = query.Include(include);
			}

			if (spec.OrderBy != null)
				query = query.OrderBy(spec.OrderBy);
			else if (spec.OrderByDescending != null)
				query = query.OrderByDescending(spec.OrderByDescending);

			return query;
		}
	}
}
