using Jobs.Domain.Common;
using Jobs.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Entities
{
    public class Skill :AggregateRoot , ISoftDelete
	{
		// ========== Properties ==========
		public string Name { get; private set; }

		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }



		// ========== Constructor ==========
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
		}

		void ISoftDelete.SoftDelete()
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}
	}
}
