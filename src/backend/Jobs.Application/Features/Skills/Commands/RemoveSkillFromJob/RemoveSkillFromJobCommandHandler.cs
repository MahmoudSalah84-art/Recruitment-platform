using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
 
namespace Jobs.Application.Features.Skills.Commands.RemoveSkillFromJob
{
	public class RemoveSkillFromJobCommandHandler : ICommandHandler<RemoveSkillFromJobCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public RemoveSkillFromJobCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(RemoveSkillFromJobCommand request, CancellationToken cancellationToken)
		{
			var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);
			if (job is null)
				return Result.Failure( "Job not found.");

			job.RemoveRequiredSkill(request.SkillId);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
