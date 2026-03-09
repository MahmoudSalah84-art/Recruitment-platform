using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Skills.Queries.GetSkillById
{
	public record GetSkillByIdQuery(string SkillId) : IQuery<SkillResponse>;

}
