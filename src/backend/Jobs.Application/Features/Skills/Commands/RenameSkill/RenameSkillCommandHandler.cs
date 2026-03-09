using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;


namespace Jobs.Application.Features.Skills.Commands.RenameSkill
{
	public class RenameSkillCommandHandler : ICommandHandler<RenameSkillCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public RenameSkillCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(RenameSkillCommand request, CancellationToken cancellationToken)
		{
			var skill = await _unitOfWork.Skills.GetByIdAsync(request.SkillId, cancellationToken);

			if (skill is null)
				return Result.Failure("Skill not found.");

			var exists = await _unitOfWork.Skills.ExistsAsync(x => x.Name == request.NewName);

			if (exists)
				return Result.Failure("A skill with this name already exists.");

			skill.Rename(request.NewName);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
