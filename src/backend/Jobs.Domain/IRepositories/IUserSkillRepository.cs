using Jobs.Domain.Entities;

namespace Jobs.Domain.IRepositories
{
	public interface IUserSkillRepository : IRepository<UserSkill>
	{
		Task<UserSkill?> FindUserSkillByIdUserAndSkillId(string userId, string skillId);
		Task<List<string>> GetSkillsByUserId(string userId, CancellationToken cancellationToken);
	}
}
