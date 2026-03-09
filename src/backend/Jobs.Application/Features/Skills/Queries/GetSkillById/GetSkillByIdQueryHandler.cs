using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Skills.Queries.GetSkillById
{
	public class GetSkillByIdQueryHandler : IQueryHandler<GetSkillByIdQuery, SkillResponse>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetSkillByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<SkillResponse>> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
		{
			var skill = await _unitOfWork.Skills.GetByIdWithRelationsAsync(request.SkillId, cancellationToken);

			if (skill is null)
				return Result<SkillResponse>.Failure( "Skill not found.");

			var respose =  new SkillResponse(
				skill.Id,
				skill.Name,
				skill.JobSkills.Count,
				skill.UserSkills.Count);

			return Result<SkillResponse>.Success(respose);

		}
	}
}
