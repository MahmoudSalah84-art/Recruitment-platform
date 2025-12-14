using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Jobs.Domain.Events.JobEvents;
using Jobs.Domain.Rules;
using Jobs.Domain.Rules.SkillRules;
using Jobs.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;


namespace Jobs.Domain.Entities
{
    public class Job : AggregateRoot
    {

		// ========= Properties =========
		public Guid CompanyId { get; private set; }
		public Company Company { get; private set; }

		public Guid HrId { get; private set; }
		public User Hr { get; private set; }

		public string Title { get; private set; }
		public string Description { get; private set; }
		public string Requirements { get; private set; }

		public int ExperienceLevel { get; private set; } //0 to 20 years
		public SalaryRange? Salary { get; private set; }

		public bool IsPublished { get; private set; }

		public DateTime? ExpirationDate { get; private set; } = null;
		public bool IsExpired => ExpirationDate.HasValue && ExpirationDate.Value <= DateTime.UtcNow;



		private readonly List<JobSkill> _requiredSkills = new();
		public IReadOnlyCollection<JobSkill> RequiredSkills => _requiredSkills.AsReadOnly();




		// ========= Constructors =========
		private Job() { }

		public Job(Guid companyId, Guid hrId, string title, SalaryRange? salary, string description, string requirements, int expLevel, DateTime? expirationDate = null)
        {
			CheckRule(new NotEmptyGuidRule(companyId));
			CheckRule(new NotEmptyGuidRule(hrId));
			CheckRule(new NotEmptyRule(title ,"title"));
			CheckRule(new ExperienceLevelRule(expLevel));

			CompanyId = companyId;
			HrId = hrId;
			Title = title;
			Salary = salary;
			Description = description ?? string.Empty;
			Requirements = requirements ?? string.Empty;
			ExperienceLevel = expLevel;
			ExpirationDate = expirationDate ?? DateTime.UtcNow.AddMonths(1);
			IsPublished = false;
			CreatedAt = DateTime.UtcNow;

			AddEvent(new JobCreatedEvent(this));

		}

		// ========= Behaviors =========
		public void Publish()
		{
			if (IsPublished)
				return;

			IsPublished = true;
			Touch();

			AddEvent(new JobPostedEvent(this));
		}

		public void UpdateDetails(string title, string description, string requirements, int expLevel, SalaryRange? salary)
        {
			CheckRule(new NotEmptyRule(title , "title"));
			CheckRule(new ExperienceLevelRule(expLevel));

			Title = title;
			Description = description ?? Description;
			Requirements = requirements ?? Requirements;
			ExperienceLevel = expLevel;
			Salary = salary;

			Touch();
			AddEvent(new JobUpdatedEvent(this));
		}

		public void AddRequiredSkill(JobSkill skill)
		{
			CheckRule(new NotNullRule<JobSkill>(skill));
			CheckRule(new SkillAlreadyAddedRule(this));

			_requiredSkills.Add(skill);
			Touch();
		}
	}
}




   