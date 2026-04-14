using Jobs.Domain.Entities;

namespace Jobs.Domain.Specifications.Jobs
{
	public class GetBySkillsIdSpecification : BaseSpecifications<Job>
	{
		public GetBySkillsIdSpecification(string SkillId, int PageNumber, int PageSize)
			: base(j => (j.RequiredSkills.Any(rs => rs.SkillId == SkillId)))
			
		{
			AddInclude(j => j.RequiredSkills);

			ApplyPagination(PageSize * (PageNumber - 1), PageSize);

			AddOrderByDescending(j => j.CreatedAt);
		}
	}
}
