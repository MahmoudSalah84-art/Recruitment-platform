
namespace Jobs.Domain.Specifications.CVJobRecommendation
{
	public class GetRecommendationsByJobSpecification : BaseSpecifications<global::Jobs.Domain.Entities.CVJobRecommendation>
	{

		public GetRecommendationsByJobSpecification(string JobId, bool OnlyActive, int PageNumber, int PageSize)
			: base(j =>
			(j.JobId == JobId && j.IsActive == OnlyActive)
			)
		{
			AddInclude(j => j.CV);

			ApplyPagination(PageSize * (PageNumber - 1), PageSize);

			AddOrderByDescending(j => j.CreatedAt);
		}
	}
}
