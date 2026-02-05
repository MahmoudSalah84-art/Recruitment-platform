using Jobs.Domain.Entities;
using Jobs.Domain.IRepository;
using Jobs.Infrastructure.Data;

namespace Jobs.Infrastructure.Repositories.Repo
{
    public class UserSkillsRepository : Repository<UserSkill>, IUserSkillRepository
	{
		public UserSkillsRepository(JobDbContext context) : base(context) { }

	}
}
