using System.Threading;
using System.Threading.Tasks;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Skills.Commands.DeleteUserSkill
{
	public class DeleteUserSkillCommandHandler : ICommandHandler<DeleteUserSkillCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteUserSkillCommandHandler(IUnitOfWork unitOfWork )
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(DeleteUserSkillCommand request, CancellationToken cancellationToken)
		{

			var skill = await _unitOfWork.UserSkills.FindUserSkillByIdUserAndSkillId(request.SeekerId, request.SkillId);
			

			if (skill is null)
				return Result.Failure("Skill not found");

			_unitOfWork.UserSkills.Remove(skill);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}

}
