using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using Jobs.Domain.Events.JobEvents;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Rules;
using Jobs.Domain.Rules.SkillRules;
using Jobs.Domain.ValueObjects;
using MediatR;


namespace Jobs.Domain.Entities
{
    public class Job : AggregateRoot , ISoftDelete
	{

		// ========= Properties =========
		public string CompanyId { get; private set; }

		public string HrId { get; private set; }

		public string Title { get; private set; }
		public string Description { get; private set; }
		public string Requirements { get; private set; }
		public EmploymentType EmploymentType { get; private set; }

		public int ExperienceLevel { get; private set; } //0 to 20 years
		public SalaryRange? Salary { get; private set; }

		public bool IsPublished { get; private set; }

		public DateTime? ExpirationDate { get; private set; } = null;
		public bool IsExpired => ExpirationDate.HasValue && ExpirationDate.Value <= DateTime.UtcNow;



		


		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }



		// Navigation Properties
		public Company Company { get; set; }
		public User HR { get; set; }

		private readonly List<JobSkill> _requiredSkills = new();
		public IReadOnlyCollection<JobSkill> RequiredSkills => _requiredSkills.AsReadOnly();


		private readonly List<JobApplication> _applications = new();
		public IReadOnlyCollection<JobApplication> Applications => _applications.AsReadOnly();


		private readonly List<CVJobRecommendation> _cVJobRecommendations = new();
		public IReadOnlyCollection<CVJobRecommendation> CVJobRecommendations => _cVJobRecommendations.AsReadOnly();

		// ========= Constructors =========
		private Job() { }
		
		public Job(string companyId,
			string hrId, string title, SalaryRange? salary, 
			string description, string requirements,
			EmploymentType employmentType,
			int expLevel, DateTime? expirationDate = null)
        {
			 
			CheckRule(new ExperienceLevelRule(expLevel));

			CompanyId = companyId;
			HrId = hrId;
			Title = title;
			Salary = salary;
			Description = description ?? string.Empty;
			Requirements = requirements ?? string.Empty;
			EmploymentType = employmentType;
			ExperienceLevel = expLevel;
			ExpirationDate = expirationDate ?? DateTime.UtcNow.AddMonths(1);
			IsPublished = false;

			CreatedAt = DateTime.UtcNow;

			AddEvent(new JobCreatedEvent(this));

		}

		// ========= Behaviors =========

		// ==================== Details ====================
		public void UpdateDetails(
			string title,
			string description,
			string requirements,
			EmploymentType employmentType,
			int experienceLevel,
			SalaryRange? salary,
			DateTime? expirationDate)
		{
			if (IsPublished)
				throw new DomainException("Cannot edit a published job. Unpublish it first.");


			Title = title;
			Description = description;
			Requirements = requirements;
			EmploymentType = employmentType;
			ExperienceLevel = experienceLevel;
			Salary = salary;
			ExpirationDate = expirationDate;

			//RaiseDomainEvent(new JobUpdatedDomainEvent(Id, CompanyId));
		}

		// ==================== Publishing ====================
		public void Publish()
		{
			if (IsPublished)
				throw new DomainException("Job is already published.");

			if (IsDeleted)
				throw new DomainException("Cannot publish a deleted job.");

			if (IsExpired)
				throw new DomainException("Cannot publish an expired job.");

			if (!_requiredSkills.Any())
				throw new DomainException("Job must have at least one required skill before publishing.");

			IsPublished = true;

			AddEvent(new JobPostedEvent(this));
			//RaiseDomainEvent(new JobPublishedDomainEvent(Id, CompanyId));
		}

		public void Unpublish()
		{
			if (!IsPublished)
				throw new DomainException("Job is not published.");

			IsPublished = false;

			//RaiseDomainEvent(new JobUnpublishedDomainEvent(Id, CompanyId));
		}

		// ==================== Expiration ====================
		public void ExtendExpiration(DateTime newExpirationDate)
		{
			if (newExpirationDate <= DateTime.UtcNow)
				throw new DomainException("New expiration date must be in the future.");

			if (ExpirationDate.HasValue && newExpirationDate <= ExpirationDate.Value)
				throw new DomainException("New expiration date must be later than the current one.");

			ExpirationDate = newExpirationDate;

			//RaiseDomainEvent(new JobExpirationExtendedDomainEvent(Id, newExpirationDate));
		}

		public void RemoveExpiration()
		{
			if (!ExpirationDate.HasValue)
				throw new DomainException("Job has no expiration date to remove.");

			ExpirationDate = null;
		}

		// ==================== HR ====================
		public void ReassignHR(string newHrId)
		{
			if (string.IsNullOrWhiteSpace(newHrId))
				throw new DomainException("HR ID is required.");

			if (HrId == newHrId)
				throw new DomainException("Job is already assigned to this HR.");

			HrId = newHrId;

			//RaiseDomainEvent(new JobHRReassignedDomainEvent(Id, newHrId));
		}

		// ==================== Required Skills ====================
		public void AddRequiredSkill(JobSkill skill)
		{

			if (_requiredSkills.Any(s => s.SkillId == skill.SkillId))
				throw new DomainException("Skill is already added to this job.");

			_requiredSkills.Add(skill);
		}

		public void RemoveRequiredSkill(string skillId)
		{
			if (IsPublished)
				throw new DomainException("Cannot remove skills from a published job. Unpublish it first.");

			var skill = _requiredSkills.FirstOrDefault(s => s.SkillId == skillId);

			if (skill is null)
				throw new DomainException("Skill not found in this job.");

			_requiredSkills.Remove(skill);
		}

		public void ClearRequiredSkills()
		{
			if (IsPublished)
				throw new DomainException("Cannot clear skills from a published job. Unpublish it first.");

			_requiredSkills.Clear();
		}

		// ==================== Applications ====================
		public void AddApplication(JobApplication application)
		{
			if (application is null)
				throw new ArgumentNullException(nameof(application));

			if (!IsPublished)
				throw new DomainException("Cannot apply for an unpublished job.");

			if (IsExpired)
				throw new DomainException("Cannot apply for an expired job.");

			if (IsDeleted)
				throw new DomainException("Cannot apply for a deleted job.");

			if (_applications.Any(a => a.ApplicantId == application.ApplicantId))
				throw new DomainException("User has already applied for this job.");

			_applications.Add(application);

			//RaiseDomainEvent(new JobApplicationReceivedDomainEvent(Id, application.UserId));
		}

		// ==================== Recommendations ====================
		public void AddCVRecommendation(CVJobRecommendation recommendation)
		{
			if (recommendation is null)
				throw new ArgumentNullException(nameof(recommendation));

			if (!IsPublished)
				throw new DomainException("Cannot add recommendations to an unpublished job.");

			if (_cVJobRecommendations.Any(r => r.CvId == recommendation.CvId))
				throw new DomainException("This CV is already recommended for this job.");

			_cVJobRecommendations.Add(recommendation);
		}

		public void ClearCVRecommendations()
		{
			_cVJobRecommendations.Clear();
		}



		// ==================== Soft Delete ====================
		public void Delete()
		{
			if (IsDeleted)
				throw new DomainException("Job is already deleted.");

			if (IsPublished)
				throw new DomainException("Cannot delete a published job. Unpublish it first.");

			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;

			//RaiseDomainEvent(new JobDeletedDomainEvent(Id, CompanyId));
		}



		public void Restore()
		{
			if (!IsDeleted)
				throw new DomainException("Job is not deleted.");

			IsDeleted = false;
			DeletedAt = null;

			//RaiseDomainEvent(new JobRestoredDomainEvent(Id, CompanyId));
		}
	
			
		void ISoftDelete.SoftDelete()
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}
	}
}




   