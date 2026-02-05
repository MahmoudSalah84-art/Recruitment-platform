using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Skills.Queries.GetUserSkills
{
	public record GetUserSkillsQuery : IQuery<List<UserSkillDto>>;

}
