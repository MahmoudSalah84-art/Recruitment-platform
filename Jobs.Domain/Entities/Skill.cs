using Jobs.Domain.Common;
using Jobs.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Entities
{
    public class Skill :BaseEntity
    {
		public string Name { get; private set; }

		private Skill() { }
		public Skill(string name)
		{
			CheckRule(new NotEmptyRule(name ,"SkillName"));
			CheckRule(new StringLengthRule(name));

			Name = name.Trim();
		}

		// ========== Behaviors ==========
		public void Rename(string newName)
		{
			CheckRule(new NotEmptyRule(newName, "SkillName"));
			CheckRule(new StringLengthRule(newName));

			Name = newName.Trim();
			Touch();
		}
	}
}
