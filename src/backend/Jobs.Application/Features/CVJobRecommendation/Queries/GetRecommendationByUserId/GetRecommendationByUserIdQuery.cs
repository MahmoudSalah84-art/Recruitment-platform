using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationById;

namespace Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationByUserId
{
	public record GetRecommendationByUserIdQuery(string UserId,int Page = 1, int PageSize = 10) : IQuery<PaginatedList<CVJobRecommendationResponse>>;
}