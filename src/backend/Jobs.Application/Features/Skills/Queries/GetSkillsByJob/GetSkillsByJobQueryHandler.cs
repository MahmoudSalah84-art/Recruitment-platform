using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Skills.Queries.GetSkillsByJob
{
	public class GetSkillsByJobQueryHandler : ICommandHandler<GetSkillsByJobQuery, List<JobSkillResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetSkillsByJobQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<List<JobSkillResponse>>> Handle(GetSkillsByJobQuery request, CancellationToken cancellationToken)
		{
			var job = await _unitOfWork.Jobs.GetByIdWithSkillsAsync(request.JobId, cancellationToken);

			if (job is null)
				return Result<List<JobSkillResponse>>.Failure("Job not found.");

			var response = job.RequiredSkills
				.Select(s => new JobSkillResponse(s.SkillId, s.Skill.Name))
				.ToList();

			return Result<List<JobSkillResponse>>.Success(response);
		}
	}
}
