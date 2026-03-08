using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Skills.Commands.AddUserSkill
{

	public class AddUserSkillCommandHandler : ICommandHandler<AddUserSkillCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public AddUserSkillCommandHandler( IUnitOfWork unitOfWork, ICurrentUserService currentUser)
		{
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result> Handle( AddUserSkillCommand request, CancellationToken cancellationToken)
		{
			if (_currentUser.UserId == string.Empty)
				return Result.Failure("User not authenticated");


			var skill = new UserSkill( _currentUser.UserId, request.SkillId );

			_unitOfWork.UserSkills.Add(skill);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}

}
