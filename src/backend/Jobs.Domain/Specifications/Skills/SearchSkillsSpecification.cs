using Jobs.Domain.Entities;

namespace Jobs.Domain.Specifications.Skills
{
	public class SearchSkillsSpecification : BaseSpecifications<Skill>
	{
		public SearchSkillsSpecification(string search, int PageNumber, int PageSize)
			: base(j => (string.IsNullOrEmpty(search) || j.Name.Contains(search.ToLower()))
			)
		{
			ApplyPagination(PageSize * (PageNumber - 1), PageSize);

			AddOrderBy(j => j.Name);
		}
	}
}
