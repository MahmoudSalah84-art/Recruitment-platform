using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Jobs.Queries.GetJobById;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.Jobs;

namespace Jobs.Application.Features.Skills.Queries.GetJobsBySkill
{
	public class GetJobsBySkillQueryHandler : IQueryHandler<GetJobsBySkillQuery, PaginatedList<JobResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetJobsBySkillQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<JobResponse>>> Handle(GetJobsBySkillQuery request, CancellationToken cancellationToken)
		{
			var skillExists = await _unitOfWork.Skills.ExistsAsync(x => x.Id == request.SkillId);
			if (!skillExists)
				return Result<PaginatedList<JobResponse>>.Failure( "Skill not found.");


			var spec = new GetBySkillsIdSpecification(
				request.SkillId, request.Page, request.PageSize);

			var Count = await _unitOfWork.Jobs.CountAsync(spec);

			var jobs = await _unitOfWork.Jobs.ListWithSpecAsync(spec);


			var response = jobs
				.Select(j => new JobResponse(
					j.Id,
					j.Title,
					j.Description,
					j.Requirements,
					j.EmploymentType.ToString(),
					j.ExperienceLevel,
					j.Salary,
					j.IsPublished,
					j.IsExpired,
					j.ExpirationDate,
					j.RequiredSkills.Select(s => s.Skill.Name).ToList(),
					j.CompanyId,
					j.HrId)
				)
				.ToList();

			var paginatedList = new PaginatedList<JobResponse>(response, Count, request.Page, request.PageSize);

			return Result<PaginatedList<JobResponse>>.Success(paginatedList);
		}
	}
}
