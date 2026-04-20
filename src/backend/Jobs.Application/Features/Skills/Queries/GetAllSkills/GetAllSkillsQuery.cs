using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Skills.Queries.GetSkillsByJob;


namespace Jobs.Application.Features.Skills.Queries.GetJobsBySkill
{
	public record GetAllSkillsQuery(
	string? Search,
	int Page = 1,
	int PageSize = 10) : IQuery<PaginatedList<JobSkillResponse>>;
}