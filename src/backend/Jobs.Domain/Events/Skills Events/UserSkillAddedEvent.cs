using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Events.Skills_Events
{

	public record UserSkillAddedEvent : DomainEvent
	{
		public UserSkillAddedEvent(string UserSkillId)
		{
			Id = UserSkillId;
		}
	}
}
