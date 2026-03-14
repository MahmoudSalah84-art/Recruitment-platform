using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Entities
{
    public class JobSkill : BaseEntity
    {
		// ========== Properties ==========
		public string JobId { get; private set; }
		public Job Job { get; private set; }

		public string SkillId { get; private set; }
		public Skill Skill { get; private set; }

		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }


		// ========== Constructor ==========
		private JobSkill() { }
		public JobSkill(string jobId , string skillId)
		{
			JobId = jobId;
			SkillId = skillId;
		}




		// ==================== Soft Delete ====================
		public void Delete()
		{
			if (IsDeleted)
				throw new DomainException("JobSkill is already deleted.");

			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}

		public void Restore()
		{
			if (!IsDeleted)
				throw new DomainException("JobSkill is not deleted.");

			IsDeleted = false;
			DeletedAt = null;
		}

	}
}




