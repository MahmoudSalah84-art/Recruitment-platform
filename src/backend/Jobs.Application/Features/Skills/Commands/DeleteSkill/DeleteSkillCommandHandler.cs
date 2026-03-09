using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;


namespace Jobs.Application.Features.Skills.Commands.DeleteSkill
{
	public class DeleteSkillCommandHandler : ICommandHandler<DeleteSkillCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteSkillCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
		{
			var skill = await _unitOfWork.Skills.GetByIdAsync(request.SkillId, cancellationToken);

			if (skill is null)
				return Result.Failure("Skill not found.");

			skill.Delete();

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
