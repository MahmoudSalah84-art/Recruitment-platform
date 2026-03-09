using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
 
namespace Jobs.Application.Features.Jobs.Commands.AddRequiredSkill
{
	public class AddRequiredSkillCommandHandler : ICommandHandler<AddRequiredSkillCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public AddRequiredSkillCommandHandler(  IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(AddRequiredSkillCommand request, CancellationToken cancellationToken)
		{
			var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);

			if (job is null)
				return Result.Failure(  "Job not found.");

			var skill = new JobSkill(request.JobId, request.SkillId, request.SkillName);

			job.AddRequiredSkill(skill);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
