

namespace Jobs.Application.Features.Skills.Queries.GetSkillById
{
	public record SkillResponse(
	string Id,
	string Name,
	int JobsCount,
	int UsersCount);
}
