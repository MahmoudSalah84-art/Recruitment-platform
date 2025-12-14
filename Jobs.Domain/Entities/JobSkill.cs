using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Jobs.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Entities
{
    public class JobSkill : BaseEntity
    {
		// ========== Properties ==========
		public Guid JobId { get; private set; }
		public Job Job { get; private set; }

		public Guid SkillId { get; private set; }
		public Skill Skill { get; private set; }


		private JobSkill() { }
		public JobSkill(Guid jobId , Guid skillId)
		{
			CheckRule(new NotEmptyGuidRule(jobId));
			CheckRule(new NotEmptyGuidRule(skillId));

			JobId = jobId;
			SkillId = skillId;
		}

	}
}




