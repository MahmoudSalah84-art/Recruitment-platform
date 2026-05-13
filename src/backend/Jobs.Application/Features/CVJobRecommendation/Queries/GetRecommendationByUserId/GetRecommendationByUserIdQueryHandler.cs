using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationById;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.CVJobRecommendation;

namespace Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationByUserId
{
	public class GetRecommendationByUserIdQueryHandler : IQueryHandler<GetRecommendationByUserIdQuery, PaginatedList<CVJobRecommendationResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetRecommendationByUserIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<CVJobRecommendationResponse>>> Handle(
			GetRecommendationByUserIdQuery request, CancellationToken cancellationToken)
		{
			//var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
			var cv = await _unitOfWork.CVs.GetByUserIdAsync(request.UserId, cancellationToken);
			if (cv is null) return Result<PaginatedList<CVJobRecommendationResponse>>.Failure("User does not have a CV.");

			//var cv = await _unitOfWork.CVs.GetByIdAsync(user.CVId, cancellationToken);
			//if (cv is null) return Result<PaginatedList<CVJobRecommendationResponse>>.Failure("CV not found.");

			var spec = new GetRecommendationsByCvSpecification(
				cv.Id, request.Page, request.PageSize);

			var Count = await _unitOfWork.CVJobRecommendations.CountAsync(spec);

			var result = await _unitOfWork.CVJobRecommendations.ListWithSpecAsync(spec);

			var response = result
				.Select(rec => new CVJobRecommendationResponse(
					rec.Id,
					rec.CvId,
					rec.JobId,
					rec.Job.Title,
					rec.Job.CompanyId,
					rec.Score,
					rec.IsActive,
					rec.DeactivatedAt))
				.ToList();

			var paginatedList = new PaginatedList<CVJobRecommendationResponse>(response, Count, request.Page, request.PageSize);

			return Result<PaginatedList<CVJobRecommendationResponse>>.Success(paginatedList);
		}
	}
}
