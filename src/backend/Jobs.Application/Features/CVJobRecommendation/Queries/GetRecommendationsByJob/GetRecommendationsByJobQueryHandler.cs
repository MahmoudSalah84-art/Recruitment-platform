using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationById;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.CVJobRecommendation;

namespace Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationsByJob
{
	public class GetRecommendationsByJobQueryHandler
	: IQueryHandler<GetRecommendationsByJobQuery, PaginatedList<CVJobRecommendationResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetRecommendationsByJobQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<CVJobRecommendationResponse>>> Handle(
			GetRecommendationsByJobQuery request,
			CancellationToken cancellationToken)
		{
			var jobExists = await _unitOfWork.Jobs.ExistsAsync(x=> x.Id ==  request.JobId );
			if (!jobExists)
				return Result<PaginatedList<CVJobRecommendationResponse>>.Failure("Job not found.");

			var spec = new GetRecommendationsByJobSpecification(
				request.JobId, request.OnlyActive, request.Page, request.PageSize);

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
