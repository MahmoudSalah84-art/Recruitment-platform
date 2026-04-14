using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Jobs.Queries.GetJobById;


namespace Jobs.Application.Features.Skills.Queries.GetJobsBySkill
{
	public record GetJobsBySkillQuery(
	string SkillId,
	int Page = 1,
	int PageSize = 10) : IQuery<PaginatedList<JobResponse>>;
}
