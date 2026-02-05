using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Interfaces;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Application.Features.Skills.Queries.GetUserSkills
{
	public class GetUserSkillsQueryHandler : IQueryHandler<GetUserSkillsQuery, List<UserSkillDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public GetUserSkillsQueryHandler( IUnitOfWork unitOfWork, ICurrentUserService currentUser)
		{
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result<List<UserSkillDto>>> Handle( GetUserSkillsQuery request, CancellationToken cancellationToken)
		{
			if (_currentUser.UserId == Guid.Empty)
				return Result<List<UserSkillDto>>.Failure("User not authenticated");

			var skills = await _unitOfWork.UserSkills.Query()
				.Where(x => x.UserId == _currentUser.UserId)
				.Select(x => new UserSkillDto(
					x.SkillId,
					x.Skill.Name)
				)
				.ToListAsync(cancellationToken);

			return Result<List<UserSkillDto>>.Success(skills);
		}
	}
}
