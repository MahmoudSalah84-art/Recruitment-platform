using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;


namespace Jobs.Application.Features.Skills.Queries.GetUserSkills
{
	public class GetUserSkillsQueryHandler : IQueryHandler<GetUserSkillsQuery, List<UserSkillDto>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetUserSkillsQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<List<UserSkillDto>>> Handle(GetUserSkillsQuery request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
			if (user == null) 
				return Result<List<UserSkillDto>>.Failure("User not found");

			var skills = await _unitOfWork.UserSkills.GetSkillsByUserId(request.UserId, cancellationToken);

			var skillsDto = skills.Select(x => new UserSkillDto
			(x)
			).ToList();

			return Result<List<UserSkillDto>>.Success(skillsDto);
		}
	}
}
