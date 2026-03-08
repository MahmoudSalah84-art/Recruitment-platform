//using Jobs.Application.Abstractions.Interfaces;
//using Jobs.Application.Abstractions.Messaging;
//using Jobs.Domain.IRepositories;
//using Jobs.Infrastructure.Repositories.UnitOfWork;
//using Microsoft.EntityFrameworkCore;

//namespace Jobs.Application.Features.Skills.Commands.DeleteUserSkill
//{
//	public class DeleteUserSkillCommandHandler : ICommandHandler<DeleteUserSkillCommand>
//	{
//		private readonly IUnitOfWork _unitOfWork;
//		private readonly ICurrentUserService _currentUser;

//		public DeleteUserSkillCommandHandler( IUnitOfWork unitOfWork, ICurrentUserService currentUser)
//		{
//			_unitOfWork = unitOfWork;
//			_currentUser = currentUser;
//		}

//		public async Task<Result> Handle( DeleteUserSkillCommand request, CancellationToken cancellationToken)
//		{
//			if (_currentUser.UserId == Guid.Empty)
//				return Result.Failure("User not authenticated");

//			var skill = await _unitOfWork.UserSkills.Query()
//				.FirstOrDefaultAsync(
//				x => x.SkillId == request.Id && x.UserId == _currentUser.UserId,
//				cancellationToken);

//			if (skill is null)
//				return Result.Failure("Skill not found");

//			_unitOfWork.UserSkills.Remove(skill);
//			await _unitOfWork.SaveChangesAsync(cancellationToken);

//			return Result.Success();
//		}
//	}

//}
