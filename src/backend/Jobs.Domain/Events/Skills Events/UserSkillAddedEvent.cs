using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.Skills_Events
{
    public class UserSkillAddedEvent : DomainEvent
    {
        public UserSkill UserSkill { get; }

        public UserSkillAddedEvent(UserSkill userSkill)
        {
            UserSkill = userSkill;
            OccurredOn = DateTime.UtcNow;
		}
    }

}
