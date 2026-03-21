using Entity = Jobs.Domain.Entities;

namespace Jobs.Domain.Specifications.User
{
	public class GetCompanyEmployeesSpecification : BaseSpecifications<Entity.User>
	{
		public GetCompanyEmployeesSpecification(string? CompanyId, int PageSize, int PageNumber)
			: base(j =>
			( j.CompanyId == CompanyId)
			)
		{

			ApplyPagination(PageSize * (PageNumber - 1), PageSize);

			AddOrderByDescending(j => j.CreatedAt);
		}
	}
}