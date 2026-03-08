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

		// Navigation Properties

		private readonly List<JobSkill> _jobSkills = new();
		public IReadOnlyCollection<JobSkill> JobSkills => _jobSkills.AsReadOnly();

		private readonly List<UserSkill> _userSkills = new();
		public IReadOnlyCollection<UserSkill> UserSkills => _userSkills.AsReadOnly();

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
