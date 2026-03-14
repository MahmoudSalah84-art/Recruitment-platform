using Jobs.Domain.Common;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Entities
{
    public class Skill :AggregateRoot  
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
			Name = name.Trim();
            IsDeleted = false;

			//skill.RaiseDomainEvent(new SkillCreatedDomainEvent(skill.Id, skill.Name));

		}

		// ========== Behaviors ==========

		// ==================== Update ====================
		public void Rename(string newName)
		{
			if (Name == newName.Trim())
				throw new DomainException("New name must be different from the current one.");

			Name = newName.Trim();

			//RaiseDomainEvent(new SkillRenamedDomainEvent(Id, Name));
		}

		// ==================== Soft Delete ====================
		public void Delete()
		{
			if (IsDeleted)
				throw new DomainException("Skill is already deleted.");

			if (_jobSkills.Any())
				throw new DomainException("Cannot delete a skill that is assigned to jobs.");

			if (_userSkills.Any())
				throw new DomainException("Cannot delete a skill that is assigned to users.");

			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;

			//RaiseDomainEvent(new SkillDeletedDomainEvent(Id));
		}

		public void Restore()
		{
			if (!IsDeleted)
				throw new DomainException("Skill is not deleted.");

			IsDeleted = false;
			DeletedAt = null;

			//RaiseDomainEvent(new SkillRestoredDomainEvent(Id));
		}

 
	}
}
