using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Skills.Commands.AddUserSkill
{
	public class AddUserSkillCommandHandler : ICommandHandler<AddUserSkillCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public AddUserSkillCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(AddUserSkillCommand request, CancellationToken cancellationToken)
		{
			var skill = new UserSkill(request.UserId, request.SkillId);
			_unitOfWork.UserSkills.Add(skill);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}

}
