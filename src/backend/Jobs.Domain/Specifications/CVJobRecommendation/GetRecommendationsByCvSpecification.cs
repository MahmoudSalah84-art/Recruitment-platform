using Jobs.Domain.Entities;

namespace Jobs.Domain.Specifications.CVJobRecommendation
{
	public class GetRecommendationsByCvSpecification : BaseSpecifications<global::Jobs.Domain.Entities.CVJobRecommendation>
	{

		public GetRecommendationsByCvSpecification(string CvId,bool OnlyActive, int PageNumber, int PageSize)
			: base(j =>
			(j.CvId == CvId && j.IsActive == OnlyActive) )
		{
			AddInclude(j => j.Job);

			ApplyPagination(PageSize * (PageNumber - 1), PageSize);

			AddOrderByDescending(j => j.CreatedAt);
		}
	}
}
