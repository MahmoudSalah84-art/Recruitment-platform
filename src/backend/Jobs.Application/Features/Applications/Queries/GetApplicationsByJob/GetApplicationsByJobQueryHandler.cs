using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Applications.Queries.GetUserApplications;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.Application;

namespace Jobs.Application.Features.Applications.Queries.GetApplicationsByJob
{
	public class GetApplicationsByJobQueryHandler : IQueryHandler<GetApplicationsByJobQuery, PaginatedList<UserApplicationDTO>>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetApplicationsByJobQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<UserApplicationDTO>>> Handle(GetApplicationsByJobQuery request, CancellationToken cancellationToken)
		{
			var jobExists = await _unitOfWork.Jobs.ExistsAsync(x => request.JobId == x.Id);
			if (!jobExists)
				return Result<PaginatedList<UserApplicationDTO>>.Failure("Job not found.");

			var spec = new UserApplicationsWithDetailsByJobIdSpec(request.JobId, request.PageSize, request.Page);

			var applications = await _unitOfWork.Applications.ListWithSpecAsync(spec);
			var Count = await _unitOfWork.Applications.CountAsync(spec);

			var dto = applications
			.Select(a => new UserApplicationDTO 
			(
				Id: a.Id,
				JobId: a.Job.Id,
				CompanyName: a.Job.Company.Name,
				MatchScore: a.MatchScore,
				Status: a.Status.ToString(),
				Created: a.CreatedAt
				)).ToList();

			var paginatedList = new PaginatedList<UserApplicationDTO>(
				dto, Count, request.Page, request.PageSize);

			return Result<PaginatedList<UserApplicationDTO>>.Success(paginatedList);
		}
	}
}
