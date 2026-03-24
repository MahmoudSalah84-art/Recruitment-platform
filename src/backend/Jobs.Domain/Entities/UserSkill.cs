using Jobs.Domain.Common;

namespace Jobs.Domain.Entities
{
    public class UserSkill : BaseEntity  
	{
		// ========== Properties ==========
		public string UserId { get; private set; }
		public User User { get; private set; }

		public string SkillId { get; private set; }
		public Skill Skill { get; private set; }

		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }

		// ========== Constructor ==========
		private UserSkill() { }

		public UserSkill(string userId, string skillId)
		{

			UserId = userId;
			SkillId = skillId;
		}

	 
	}
}
