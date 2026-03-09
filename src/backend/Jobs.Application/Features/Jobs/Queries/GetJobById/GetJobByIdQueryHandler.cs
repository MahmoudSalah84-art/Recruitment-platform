using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Jobs.Queries.GetJobById
{
	public class GetJobByIdQueryHandler : IQueryHandler<GetJobByIdQuery, JobResponse>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetJobByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<JobResponse>> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
		{
			var job = await _unitOfWork.Jobs.GetByIdWithSkillsAsync(request.JobId, cancellationToken);

			if (job is null)
				return Result<JobResponse>.Failure("Job not found.");

			var jobResponse = new JobResponse(job.Id,
				job.Title,
				job.Description,
				job.Requirements,
				job.EmploymentType.ToString(),
				job.ExperienceLevel,
				job.Salary,
				job.IsPublished,
				job.IsExpired,
				job.ExpirationDate,
				[.. job.RequiredSkills.Select(s => s.Skill.Name)],
				job.CompanyId,
				job.HrId);

			return Result<JobResponse>.Success(jobResponse);

		}
	}
}
