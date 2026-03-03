using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Jobs.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Entities
{
    public class JobSkill : BaseEntity , ISoftDelete
    {
		// ========== Properties ==========
		public Guid JobId { get; private set; }
		public Job Job { get; private set; }

		public Guid SkillId { get; private set; }
		public Skill Skill { get; private set; }

		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }


		// ========== Constructor ==========
		private JobSkill() { }
		public JobSkill(Guid jobId , Guid skillId)
		{
			CheckRule(new NotEmptyGuidRule(jobId));
			CheckRule(new NotEmptyGuidRule(skillId));

			JobId = jobId;
			SkillId = skillId;
		}

		void ISoftDelete.SoftDelete()
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}
	}
}




