using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationById
{
	public class GetRecommendationByIdQueryHandler : IQueryHandler<GetRecommendationByIdQuery, CVJobRecommendationResponse>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetRecommendationByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CVJobRecommendationResponse>> Handle(GetRecommendationByIdQuery request, CancellationToken cancellationToken)
		{
			var rec = await _unitOfWork.CVJobRecommendations.GetByIdWithDetailsAsync(request.RecommendationId, cancellationToken);

			if (rec is null)
				return Result<CVJobRecommendationResponse>.Failure("Recommendation not found.");

			var Respons = new CVJobRecommendationResponse(
				rec.Id,
				rec.CvId,
				rec.JobId,
				rec.Job.Title,
				rec.Job.CompanyId,
				rec.Score,
				rec.IsActive,
				rec.DeactivatedAt);

			return Result<CVJobRecommendationResponse>.Success(Respons);
		}
	}
}
