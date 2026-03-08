using Jobs.Domain.Common;
using Jobs.Domain.Events.Company_Events;
using Jobs.Domain.Events.JobEvents;
using Jobs.Domain.Rules;
using Jobs.Domain.Rules.CompanyRoles;
using Jobs.Domain.Rules.UserRules;
using Jobs.Domain.ValueObjects;
using Jobs.Domain.ValueObjects.YourProject.Domain.ValueObjects;

namespace Jobs.Domain.Entities
{
    public class Company : AggregateRoot , ISoftDelete
	{
		// ========== Properties ==========
		public string Name { get; private set; }
		public Email Email { get; private set; }
		public string Industry { get; private set; }
        public Address CompanyAddress { get; private set; }
        public int EmployeesCount { get; private set; }
        public string Description { get; private set; } = string.Empty;
		public string LogoUrl { get; private set; } = string.Empty;

		private readonly List<User> _employees = new();
		public IReadOnlyCollection<User> Employees => _employees; // current user

		private readonly  List<Job> _jobs = new();
		//if you don't wrirte AsReadOnly(), the consumer can cast it back to List<Job> and modify it.
		public IReadOnlyCollection<Job> Jobs => _jobs.AsReadOnly();

		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }


		// ========== Constructor ==========
		private Company(){}

		public Company(string name,bool isNameExists, 
			string email,bool isEmailExists, 
			string industry,string Country, string city,
			string Street, string BuildingNumber, string postalCode,
			string? logoUrl = default, string? description = default)
		{
			CheckRule(new CompanyNameMustBeUniqueRule(isNameExists));
			CheckRule(new UserEmailMustBeUniqueRule(isEmailExists));

			Name = name;
			Email = Email.Create(email);
			Industry = industry;
			CompanyAddress = Address.Create(Country, city, Street, BuildingNumber, postalCode);
			Description = description ?? string.Empty;
			LogoUrl = logoUrl ?? string.Empty;

			CreatedAt = DateTime.UtcNow;
			UpdatedAt = CreatedAt;

			AddEvent(new CompanyCreatedEvent(this));
		}

		// ========== Behaviors ==========
		public void UpdateProfile(string name, string description, string industry, int employeesCount, string logoUrl, Address address)
		{
			CheckRule(new NotEmptyRule(name, nameof(name)));
			CheckRule(new NotEmptyRule(industry, nameof(industry)));

			Name = name ?? Name;
			Description = description ?? Description;
			Industry = industry ?? Industry;
			LogoUrl = logoUrl ?? LogoUrl;
			CompanyAddress = address;
			EmployeesCount = employeesCount;
			UpdatedAt = DateTime.UtcNow;
			AddEvent(new CompanyInfoUpdatedEvent(this.Id));
		}
		public void AddEmployee(User user)
		{
			CheckRule(new NotNullRule<User>(user ));
			
			if (!_employees.Contains(user))
			{
				_employees.Add(user);
			}
		}

		public void RemoveEmployee(User user)
		{
			CheckRule(new NotNullRule<User>(user));

			if (_employees.Contains(user))
			{
				_employees.Remove(user);
			}
		}

		public void AddJob(Job job)
		{
			CheckRule(new NotNullRule<Job>(job));
			CheckRule(new CompanyCannotPostMoreThanNJobsRule(this));

			_jobs.Add(job);
			
			AddEvent(new JobCreatedEvent(job));
		}
		
		
        void ISoftDelete.SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        
    }
}













