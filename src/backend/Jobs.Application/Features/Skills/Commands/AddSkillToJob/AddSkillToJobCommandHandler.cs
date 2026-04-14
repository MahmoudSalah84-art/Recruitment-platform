using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;


namespace Jobs.Application.Features.Skills.Commands.AddSkillToJob
{
	public class AddSkillToJobCommandHandler : ICommandHandler<AddSkillToJobCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public AddSkillToJobCommandHandler(
			IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(AddSkillToJobCommand request, CancellationToken cancellationToken)
		{
			var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);
			if (job is null)
				return Result.Failure("Job not found.");

			var skill = await _unitOfWork.Skills.GetByIdAsync(request.SkillId, cancellationToken);
			if (skill is null)
				return Result.Failure( "Skill not found.");

			var jobSkill = new JobSkill(request.JobId, request.SkillId);

			job.AddRequiredSkill(jobSkill);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
