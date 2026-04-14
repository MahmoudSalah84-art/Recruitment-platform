using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Jobs.Queries.GetJobById;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.Jobs;

namespace Jobs.Application.Features.Jobs.Queries.GetJobsByCompany
{
	public class GetJobsByCompanyQueryHandler : IQueryHandler<GetJobsByCompanyQuery, PaginatedList<JobResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetJobsByCompanyQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

	 
		public async Task<Result<PaginatedList<JobResponse>>> Handle(GetJobsByCompanyQuery request, CancellationToken cancellationToken)
		{
			var spec = new GetByCompanyIdSpecification(
				request.CompanyId, request.Page, request.PageSize);

			var Count = await _unitOfWork.Jobs.CountAsync(spec);

			var jobs = await _unitOfWork.Jobs.ListWithSpecAsync(spec);

			var response = jobs.Select(job => new JobResponse(
				job.Id,
				job.Title,
				job.Description,
				job.Requirements,
				job.EmploymentType.ToString(),
				job.ExperienceLevel,
				job.Salary,
				job.IsPublished,
				job.IsExpired,
				job.ExpirationDate,
				job.RequiredSkills.Select(s => s.Skill.Name).ToList(),
				job.CompanyId,
				job.HrId)).ToList();

			var paginatedList = new PaginatedList<JobResponse>(response, Count, request.Page, request.PageSize);
			return Result<PaginatedList<JobResponse>>.Success(paginatedList);

		}


	}
}
