using Jobs.Domain.Entities;
using Jobs.Domain.Repository.Repo;
using Jobs.Infrastructure.Data;

namespace Jobs.Infrastructure.Repositories.Repo
{
    public class JobSkillsRepository : Repository<JobSkill>, IJobSkillRepository
	{
		public JobSkillsRepository(JobDbContext context) : base(context) { }
	}
}
