using Jobs.Domain.Common;
using Jobs.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Entities
{
    public class UserSkill : BaseEntity , ISoftDelete
	{
		// ========== Properties ==========
		public Guid UserId { get; private set; }
		public User User { get; private set; }

		public Guid SkillId { get; private set; }
		public Skill Skill { get; private set; }

		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }

		// ========== Constructor ==========
		private UserSkill() { }

		public UserSkill(Guid userId, Guid skillId)
		{
			CheckRule(new NotEmptyGuidRule(userId));
			CheckRule(new NotEmptyGuidRule(skillId));

			UserId = userId;
			SkillId = skillId;
		}

		void ISoftDelete.SoftDelete()
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}
	}
}
