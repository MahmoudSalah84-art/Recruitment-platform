using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Jobs.Queries.GetJobById;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.Jobs;

namespace Jobs.Application.Features.Jobs.Queries.SearchJobs
{
	public class JobsWithFiltersSpecificationQueryHandler : IQueryHandler<JobsWithFiltersSpecificationQuery, PaginatedList<JobResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public JobsWithFiltersSpecificationQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		public async Task<Result<PaginatedList<JobResponse>>> Handle(JobsWithFiltersSpecificationQuery request, CancellationToken cancellationToken)
		{
			var spec = new JobsWithFiltersSpecification(
				request.Search, request.PageSize, request.PageNumber,
				request.Country, request.City, request.MinExperience,
				request.Type,request.OnlyPublished, request.PostedInDays);

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

			PaginatedList<JobResponse> paginatedList = PaginatedList<JobResponse>.Create(response, Count, request.PageNumber, request.PageSize);
			return Result<PaginatedList<JobResponse>>.Success(paginatedList);
		}
	}
}