using Entity = Jobs.Domain.Entities;

namespace Jobs.Domain.Specifications.Company
{
	public class CommpaniesWithDetilesSpecification : BaseSpecifications<Entity.Company>
	{
		public CommpaniesWithDetilesSpecification(string? name , string? industry, int PageSize, int PageNumber)
			: base(j =>
			((string.IsNullOrEmpty(name) || j.Name.Contains(name.ToLower()) &&
			(string.IsNullOrEmpty(industry) || j.Industry.Contains(industry.ToLower())
			))))
		{
			AddInclude(j => j.Employees);
			AddInclude(j => j.Jobs);

			ApplyPagination(PageSize * (PageNumber - 1), PageSize);


			AddOrderByDescending(j => j.CreatedAt);
		}
	}
}