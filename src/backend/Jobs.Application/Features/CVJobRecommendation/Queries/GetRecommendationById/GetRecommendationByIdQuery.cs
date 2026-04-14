using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationById
{
	public record GetRecommendationByIdQuery(string RecommendationId) : IQuery< CVJobRecommendationResponse>;

	
}
