using Jobs.Domain.Common;
using System.Linq.Expressions;

namespace Jobs.Domain.Specifications
{
	public class BaseSpecifications<T> : ISpecification<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>>? Criteria { get; set; }
		public List<Expression<Func<T, object>>> Includes { get; private set; } = new();
		public List<string> thenIncludes { get; private set; } = new();

		public Expression<Func<T, object>>? OrderBy { get; private set; }
		public Expression<Func<T, object>>? OrderByDescending { get; private set; }
		public int Take { get; set; }
		public int Skip { get; set; }
		public bool IsPagingEnabled { get; set; }

		 
		public BaseSpecifications() { }
		protected BaseSpecifications(Expression<Func<T, bool>> criteria) 
			=> Criteria = criteria;

		protected void AddInclude(Expression<Func<T, object>> includeExpression)
			=> Includes.Add(includeExpression);

		protected void AddThenInclude(string _thenIncludes)
			=> thenIncludes.Add(_thenIncludes);
		public void AddOrderBy(Expression<Func<T, object>> OrderByExpression) 
			=> OrderBy = OrderByExpression;
		public void AddOrderByDescending(Expression<Func<T, object>> OrderByDescExpression) 
			=> OrderByDescending = OrderByDescExpression;
		public void ApplyPagination(int skip, int take) 
			=> (IsPagingEnabled, Skip, Take) = (true, skip, take);
	}
}
