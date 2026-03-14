using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using Jobs.Domain.Events.Events;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Rules;
using Jobs.Domain.Rules.UserRules;
using Jobs.Domain.ValueObjects;

namespace Jobs.Domain.Entities
{
    public class User : AggregateRoot  
    {
		// ========= Properties =========
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public Email Email { get; private set; }
        public UserRole Role { get; private set; }
        public PhoneNumber? PhoneNumber { get; private set; }
        public string ProfilePictureUrl { get; private set; } = string.Empty;
		public string Bio { get; private set; } = string.Empty;
		public string? LinkedInUrl { get; private set; }
		public string? GitHubUrl { get; private set; }
		public string? PortfolioUrl { get; private set; }

		public string? CompanyId { get; private set; }
        public Company Company { get; private set; }

        public bool IsVerified { get; private set; }


		private readonly List<UserSkill> _skills = new();
		public IReadOnlyCollection<UserSkill> Skills => _skills.AsReadOnly();

		public CV CV { get; private set; }


		private readonly List<JobApplication> _applications = new();
		public IReadOnlyCollection<JobApplication> Applications => _applications.AsReadOnly();



		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }
		// ========= Constructors =========
		private User() { }
	
		public User(string firstName, string lastName, string email, bool isEmailExists, string? phoneNumber = null, string bio = "")
		{
			CheckRule(new UserEmailMustBeUniqueRule(isEmailExists));

			FirstName = firstName;
			LastName = lastName;
			Email =  Email.Create(email);
			PhoneNumber = PhoneNumber.Create(phoneNumber);
			Bio = bio;

			IsVerified = false;
			CreatedAt = DateTime.UtcNow;
			UpdatedAt = CreatedAt;

			AddEvent(new UserRegisteredEvent(this));
		}

		// ========= Behaviors =========
		public void Verify()
		{
			if (IsVerified) return;

			IsVerified = true;
			AddEvent(new UserVerifiedEvent(this));
		}
		public void UpdateEmail(Email newEmail , bool isEmailExists)
        {
			CheckRule(new NotNullRule<Email>(newEmail));
			CheckRule(new EmailFormatRule(newEmail));
			CheckRule(new UserEmailMustBeUniqueRule(isEmailExists));

			Email = newEmail;
			UpdatedAt = DateTime.UtcNow;
		}
        public void UpdatePhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
			UpdatedAt = DateTime.UtcNow;

		}

		public void UpdateProfile(
		string firstName,
		string lastName,
		string? bio,
		string? linkedInUrl,
		string? gitHubUrl,
		string? portfolioUrl)
		{
			if (string.IsNullOrWhiteSpace(firstName))
				throw new DomainException("First name is required.");

			if (string.IsNullOrWhiteSpace(lastName))
				throw new DomainException("Last name is required.");

			FirstName = firstName;
			LastName = lastName;
			Bio = bio ?? string.Empty;
			LinkedInUrl = linkedInUrl;
			GitHubUrl = gitHubUrl;
			PortfolioUrl = portfolioUrl;

			//RaiseDomainEvent(new UserProfileUpdatedDomainEvent(Id));
		}

		public void UpdateProfilePicture(string profilePictureUrl)
		{

			ProfilePictureUrl = profilePictureUrl;
		}
		// ==================== Email ====================
		public void ChangeEmail(Email newEmail)
		{
			if (Email == newEmail)
				throw new DomainException("New email must be different from the current one.");

			Email = newEmail;
			IsVerified = false;  

			//RaiseDomainEvent(new UserEmailChangedDomainEvent(Id, newEmail.Value));
		}


		// ==================== Company ====================
		public void AssignToCompany(string companyId)
		{

			CompanyId = companyId;
		}

		public void RemoveFromCompany()
		{
			if (CompanyId is null)
				throw new DomainException("User is not assigned to any company.");

			CompanyId = null;
			Company = null!;
		}

		// ==================== Skills ====================
		public void AddSkill(UserSkill skill)
		{
			if (_skills.Any(s => s.SkillId == skill.SkillId))
				throw new DomainException("Skill already added.");

			_skills.Add(skill);
		}

		public void RemoveSkill(string skillId)
		{
			var skill = _skills.FirstOrDefault(s => s.SkillId == skillId);

			if (skill is null)
				throw new DomainException("Skill not found.");

			_skills.Remove(skill);
		}
		public void ClearSkills()
		{
			_skills.Clear();
		}

		// ==================== CV ====================
		public void UploadCV(CV cv)
		{
			CV = cv;

			//RaiseDomainEvent(new UserCVUploadedDomainEvent(Id));
		}


		public void RemoveCV()
		{
			if (CV is null)
				throw new DomainException("No CV to remove.");

			CV = null!;
		}

		// ==================== Job Applications ====================
		public void AddApplication(JobApplication application)
		{
			CheckRule(new CandidateCannotApplyTwiceRule(this, application.JobId));
			CheckRule(new CannotApplyToExpiredJobRule(application.Job));

			UpdatedAt = DateTime.UtcNow;

			_applications.Add(application);

			//RaiseDomainEvent(new UserAppliedForJobDomainEvent(Id, application.JobId));
		}
		

		public void WithdrawApplication(string jobId)
		{
			var application = _applications.FirstOrDefault(a => a.JobId == jobId);

			if (application is null)
				throw new DomainException("Application not found.");

			if (application.Status != ApplicationStatus.Pending)
				throw new DomainException("Cannot withdraw a processed application.");

			_applications.Remove(application);

			//RaiseDomainEvent(new UserWithdrewApplicationDomainEvent(Id, jobId));
		}

		// ==================== Soft Delete ====================
		public void Delete()
		{
			if (IsDeleted)
				throw new DomainException("User is already deleted.");

			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;

			//RaiseDomainEvent(new UserDeletedDomainEvent(Id));
		}

		public void Restore()
		{
			if (!IsDeleted)
				throw new DomainException("User is not deleted.");

			IsDeleted = false;
			DeletedAt = null;
		}
	 


 
    }
}

