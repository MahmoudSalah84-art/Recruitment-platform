using Jobs.Domain.Common;
using Jobs.Domain.Exceptions;

namespace Jobs.Domain.Entities
{
    public class JobSkill : BaseEntity
    {
		// ========== Properties ==========
		public string JobId { get; private set; }
		public Job Job { get; private set; }

		public string SkillId { get; private set; }
		public Skill Skill { get; private set; }

		// ========== Constructor ==========
		private JobSkill() { }
		public JobSkill(string jobId , string skillId)
		{
			JobId = jobId;
			SkillId = skillId;
		}




		// ==================== Soft Delete ====================
		public void Restore()
		{
			if (!IsDeleted)
				throw new DomainException("JobSkill is not deleted.");

			IsDeleted = false;
			DeletedAt = null;
		}

	}
}




