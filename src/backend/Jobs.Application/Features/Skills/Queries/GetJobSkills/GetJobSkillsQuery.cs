using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Skills.Queries.GetSkillsByJob
{
	public record GetJobSkillsQuery(string JobId) : ICommand<List<JobSkillResponse>>;
}
