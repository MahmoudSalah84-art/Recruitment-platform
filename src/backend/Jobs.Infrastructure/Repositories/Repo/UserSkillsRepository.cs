using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using Jobs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Infrastructure.Repositories.Repo
{
    public class UserSkillsRepository : Repository<UserSkill>, IUserSkillRepository
	{
		public UserSkillsRepository(JobDbContext context) : base(context) { }

		public async Task<UserSkill?> FindUserSkillByIdUserAndSkillId(string userId, string skillId)
		{
			return await _set.FirstOrDefaultAsync(x => x.UserId == userId && x.SkillId == skillId);
		}

		public Task<List<string>> GetSkillsByUserId(string userId, CancellationToken cancellationToken)
		{
			return _set.Where(x => x.UserId == userId)
                       .Select(x => x.Skill.Name)
                       .ToListAsync(cancellationToken);
		}
	}
}
