using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.SkillRules
{
	public class SkillAlreadyAddedRule : IBusinessRule
	{
		private readonly IEnumerable<JobSkill> _skills;
		private readonly string _skillId;

		public SkillAlreadyAddedRule(Job job)
		{
			_skills = job.RequiredSkills;
			_skillId = job.Id;
		}

		public bool IsBroken() =>_skills.Any(s => s.SkillId == _skillId);

		public string Message =>"This skill is already added to the job.";
	}
}
