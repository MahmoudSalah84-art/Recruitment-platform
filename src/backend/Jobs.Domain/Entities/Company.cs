using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using Jobs.Domain.Events.Company_Events;
using Jobs.Domain.Events.JobEvents;
using Jobs.Domain.Exceptions;
using Jobs.Domain.Rules;
using Jobs.Domain.Rules.CompanyRoles;
using Jobs.Domain.Rules.UserRules;
using Jobs.Domain.ValueObjects;
using Jobs.Domain.ValueObjects.YourProject.Domain.ValueObjects;

namespace Jobs.Domain.Entities
{
    public class Company : AggregateRoot
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
		public void UpdateProfile( string? description, string? industry, int employeesCount, Address? address)
		{
			Description = description ?? Description;
			Industry = industry?.Trim() ?? Industry;
			CompanyAddress = address ?? CompanyAddress;
			EmployeesCount = employeesCount;
			UpdatedAt = DateTime.UtcNow;

			AddEvent(new CompanyInfoUpdatedEvent(this.Id));
		}

		public void UpdateLogo(string logoUrl)
		{
			LogoUrl = logoUrl;
		}


		// ==================== Employees ====================
		public void AddEmployee(User employee)
		{
			if (_employees.Any(e => e.Id == employee.Id))
				throw new DomainException("Employee is already part of this company.");

			_employees.Add(employee);

			//RaiseDomainEvent(new CompanyEmployeeAddedDomainEvent(Id, employee.Id));
		}

		public void RemoveEmployee(string employeeId)
		{
			var employee = _employees.FirstOrDefault(e => e.Id == employeeId);

			if (employee is null)
				throw new DomainException("Employee not found in this company.");

			var hasActiveJobs = _jobs.Any(j =>
				j.HrId == employeeId && j.IsPublished && !j.IsExpired);

			if (hasActiveJobs)
				throw new DomainException("Cannot remove an employee who owns active published jobs.");

			_employees.Remove(employee);

			//RaiseDomainEvent(new CompanyEmployeeRemovedDomainEvent(Id, employeeId));
		}

		// ==================== Jobs ====================
		public void AddJob(Job job)
		{
			CheckRule(new CompanyCannotPostMoreThanNJobsRule(this));

			ArgumentNullException.ThrowIfNull(job);

			if (IsDeleted)
				throw new DomainException("Cannot add jobs to a deleted company.");

			if (_jobs.Any(j => j.Id == job.Id))
				throw new DomainException("Job is already associated with this company.");

			_jobs.Add(job);

			AddEvent(new JobCreatedEvent(job));

			//RaiseDomainEvent(new CompanyJobAddedDomainEvent(Id, job.Id));
		}

		public void RemoveJob(string jobId)
		{
			var job = _jobs.FirstOrDefault(j => j.Id == jobId);

			if (job is null)
				throw new DomainException("Job not found in this company.");

			if (job.IsPublished)
				throw new DomainException("Cannot remove a published job. Unpublish it first.");

			_jobs.Remove(job);
		}

		

	}
}













