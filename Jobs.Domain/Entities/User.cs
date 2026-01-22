using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using Jobs.Domain.Events.Events;
using Jobs.Domain.Rules;
using Jobs.Domain.Rules.UserRules;
using Jobs.Domain.ValueObjects;

namespace Jobs.Domain.Entities
{
    public class User : AggregateRoot , ISoftDelete
    {
		// ========= Properties =========
		public string FullName { get; private set; }
        public Email Email { get; private set; }
        public UserRole Role { get; private set; }
        public PhoneNumber? PhoneNumber { get; private set; }
        public string ProfilePictureUrl { get; private set; } = string.Empty;
		public string Bio { get; private set; } = string.Empty;
		public string? LinkedInUrl { get; private set; }
		public string? GitHubUrl { get; private set; }
		public string? PortfolioUrl { get; private set; }

		public Guid? CompanyId { get; private set; }
        public Company Company { get; private set; }

        public bool IsVerified { get; private set; }


		private readonly List<UserSkill> _skills = new();
		public IReadOnlyCollection<UserSkill> Skills => _skills.AsReadOnly();


		public readonly List<CV> _cvs = new();
		public IReadOnlyCollection<CV> CVs => _cvs.AsReadOnly();


		private readonly List<JobApplication> _applications = new();
		public IReadOnlyCollection<JobApplication> Applications => _applications.AsReadOnly();


		private readonly List<UserExperience> _experience = new();
		public IReadOnlyCollection<UserExperience> Experience => _experience.AsReadOnly();


		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }
		// ========= Constructors =========
		private User() { }
		public User(string fullName, Email email, UserRole role , Func<string, bool> emailExists)
		{
			CheckRule(new NotEmptyRule(fullName , "name"));
			CheckRule(new StringLengthRule(fullName));
			CheckRule(new NotNullRule<Email>(email));
			CheckRule(new EmailFormatRule(email));
			CheckRule(new UserEmailMustBeUniqueRule(emailExists, email.Value));


			FullName = fullName ;
			Email = email ;
			Role = role;
			IsVerified = false;
			CreatedAt = DateTime.UtcNow;

			AddEvent(new UserRegisteredEvent(this));
		}

		// ========= Behaviors =========
		public void Verify()
		{
			if (IsVerified) return;

			IsVerified = true;
			AddEvent(new UserVerifiedEvent(this));
		}
		public void UpdateEmail(Email newEmail , Func<string, bool> emailExists)
        {
			CheckRule(new NotNullRule<Email>(newEmail));
			CheckRule(new EmailFormatRule(newEmail));
			CheckRule(new UserEmailMustBeUniqueRule(emailExists, newEmail.Value));

			Email = newEmail;
		}
        public void UpdatePhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
			
		}

		public void UpdateProfile(string fullName, String email, String phoneNumber, string bio, string profilePictureUrl)
		{
			FullName = fullName;
			Email.Create(email);
			PhoneNumber.Create(phoneNumber);
			Bio = bio ?? Bio;
			ProfilePictureUrl = profilePictureUrl ?? ProfilePictureUrl;
		}

		public void AddSkill(UserSkill skill)
		{
			CheckRule(new NotNullRule<UserSkill>(skill));
			if (_skills.Any(s => s.SkillId == skill.SkillId))
				return;

			_skills.Add(skill);
		}

		public void AddCV(CV cv)
		{
			CheckRule(new NotNullRule<CV>(cv));
			_cvs.Add(cv);
		}

		public void AddApplication(JobApplication application)
		{
			CheckRule(new NotNullRule<JobApplication>(application));
			CheckRule(new CandidateCannotApplyTwiceRule(this, application.JobId));
			CheckRule(new CannotApplyToExpiredJobRule(application.Job));

			_applications.Add(application);
		}

		void ISoftDelete.SoftDelete()
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}

	}
}

