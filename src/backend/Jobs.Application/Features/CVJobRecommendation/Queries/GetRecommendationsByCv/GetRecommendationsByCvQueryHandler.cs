using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationById;
using Jobs.Application.Features.Jobs.Queries.GetJobById;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.CVJobRecommendation;
using Jobs.Domain.Specifications.Jobs;


namespace Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationsByCv
{
	public class GetRecommendationsByCvQueryHandler : IQueryHandler<GetRecommendationsByCvQuery, PaginatedList<CVJobRecommendationResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetRecommendationsByCvQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<CVJobRecommendationResponse>>> Handle(
			GetRecommendationsByCvQuery request, CancellationToken cancellationToken)
		{
			var cvExists = await _unitOfWork.CVs.ExistsAsync(x => x.Id == request.CvId);
			if (!cvExists)
				return Result<PaginatedList<CVJobRecommendationResponse>>.Failure("CV not found.");

			var spec = new GetRecommendationsByCvSpecification(
				request.CvId,request.OnlyActive, request.Page, request.PageSize);

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
