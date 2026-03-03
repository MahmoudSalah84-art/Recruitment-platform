using Jobs.Domain.Entities;

namespace Jobs.Domain.Specifications.Jobs
{
	public class ActiveJobsWithCompanySpec : BaseSpecifications<Job>
	{
		public ActiveJobsWithCompanySpec(Guid companyId) : base(j => j.CompanyId == companyId && !j.IsExpired)
		{
			AddInclude(j => j.Company);

			AddOrderBy(j => j.CreatedAt);
		}
	}
}
