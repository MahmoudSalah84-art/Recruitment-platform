using Jobs.Domain.Entities;

namespace Jobs.Domain.Specifications.Jobs
{
	public class GetByCompanyIdSpecification : BaseSpecifications<Job>
	{
		public GetByCompanyIdSpecification(string CompanyId, int PageNumber, int PageSize)
			: base(j =>
			(j.CompanyId == CompanyId)
			)
		{
			AddInclude(j => j.Company);

			ApplyPagination(PageSize * (PageNumber - 1), PageSize);

			AddOrderByDescending(j => j.CreatedAt);
		}
	}
}