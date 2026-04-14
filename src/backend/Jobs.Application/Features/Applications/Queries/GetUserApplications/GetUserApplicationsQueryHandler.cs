using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Applications.Queries.GetUserApplications;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.Application;

namespace Jobs.Application.Features.Applications.Queries.GetMyApplications
{

	public class GetUserApplicationsQueryHandler : IQueryHandler<GetUserApplicationsQuery, PaginatedList<UserApplicationDTO>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetUserApplicationsQueryHandler(IUnitOfWork unitOfWork )
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<UserApplicationDTO>>> Handle(GetUserApplicationsQuery request, CancellationToken cancellationToken)
		{

			var spec = new UserApplicationsWithDetailsSpec(request.ApplicantId, request.PageSize, request.PageNumber);

			var applications = await _unitOfWork.Applications.ListWithSpecAsync(spec);
			var Count = await _unitOfWork.Applications.CountAsync(spec);

			var dto = applications.Select(a => new UserApplicationDTO
				(
				Id: a.Id,
				JobId: a.Job.Id,
				CompanyName: a.Job.Company.Name,
				MatchScore: a.MatchScore,
				Status: a.Status.ToString(),
				Created: a.CreatedAt
				)).ToList();

			var paginatedList = new PaginatedList<UserApplicationDTO>(
				dto,
				Count,
				request.PageNumber,
				request.PageSize);

			return Result<PaginatedList<UserApplicationDTO>>.Success(paginatedList);
		}
	}
}
