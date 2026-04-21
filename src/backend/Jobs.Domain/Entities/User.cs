using Jobs.Domain.Common;
using Jobs.Domain.Events.Events;
using Jobs.Domain.Exceptions;

namespace Jobs.Domain.Entities
{
    public class User : AggregateRoot  
    {
		// ========= Properties =========
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
        public string ProfilePictureUrl { get; private set; } = string.Empty;
		public string Bio { get; private set; } = string.Empty;

		public string? CompanyId { get; private set; }
        public Company Company { get; private set; }



		private readonly List<UserSkill> _skills = new();
		public IReadOnlyCollection<UserSkill> Skills => _skills.AsReadOnly();

		public string? CVId { get; private set; }
		public CV CV { get; private set; }


		private readonly List<JobApplication> _applications = new();
		public IReadOnlyCollection<JobApplication> Applications => _applications.AsReadOnly();

		// ========= Constructors =========
		private User() { }
	
		public User(string firstName, string lastName, string bio = "")
		{

			FirstName = firstName;
			LastName = lastName;
			
			Bio = bio;


			AddEvent(new UserRegisteredEvent(this.Id));
		}

		// ========= Behaviors =========

		//public void UpdateEmail(Email newEmail , bool isEmailExists)
  //      {
		//	CheckRule(new NotNullRule<Email>(newEmail));
		//	CheckRule(new EmailFormatRule(newEmail));
		//	CheckRule(new UserEmailMustBeUniqueRule(isEmailExists));

		//	Email = newEmail;
		//}

		public void UpdateProfile(
		string firstName,
		string lastName,
		string? bio)
		{
			if (string.IsNullOrWhiteSpace(firstName))
				throw new DomainException("First name is required.");

			if (string.IsNullOrWhiteSpace(lastName))
				throw new DomainException("Last name is required.");

			FirstName = firstName;
			LastName = lastName;
			Bio = bio ?? string.Empty;
			

			//AddEvent(new UserProfileUpdatedDomainEvent(Id));

		}

		public void UpdateProfilePicture(string profilePictureUrl)
		{

			ProfilePictureUrl = profilePictureUrl;

			//AddEvent(new UpdateProfilePictureDomainEvent(Id));

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

		

		//public void Restore()
		//{
		//	if (!IsDeleted)
		//		throw new DomainException("User is not deleted.");

		//	IsDeleted = false;
		//	DeletedAt = null;
		//}
    }
}

