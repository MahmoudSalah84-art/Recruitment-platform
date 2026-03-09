using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Jobs.Infrastructure.Repositories.Repo
{
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        public SkillRepository(JobDbContext context) : base(context) { }

		public async Task<Skill?> GetByIdWithRelationsAsync(string id, CancellationToken cancellationToken)
		{
			return await _set
				.Include(s => s.JobSkills)
				.Include(s => s.UserSkills)
				.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
		}
	}
}
