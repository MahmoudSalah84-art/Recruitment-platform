using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Jobs.Domain.Enums;
using Jobs.Domain.Rules;
using Jobs.Domain.Rules.UserExperience;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Jobs.Domain.Entities
{
    public class UserExperience : AggregateRoot
    {

		// ========= Properties =========
		public Guid UserId { get; private set; }
		public User User { get; private set; }

		public Guid CompanyId { get; private set; }
		public Company Company { get; private set; }

		public string JobTitle { get; private set; }
		public EmploymentType EmploymentType { get; private set; }

		public DateTime StartDate { get; private set; }
		public DateTime? EndDate { get; private set; }
		public bool IsCurrent { get; private set; }

		public string Description { get; private set; }

		// ========= Constructors =========
		private UserExperience() { }

		public UserExperience(
			Guid userId,
			string jobTitle,
			Guid companyId,
			EmploymentType employmentType,
			DateTime startDate,
			DateTime? endDate,
			bool isCurrent,
			string description)
		{
			CheckRule(new NotEmptyRule(jobTitle, nameof(JobTitle)));
			CheckRule(new ExperienceDateRangeRule(startDate, endDate, isCurrent));

			UserId = userId;
			JobTitle = jobTitle;
			CompanyId = companyId;
			EmploymentType = employmentType;
			StartDate = startDate;
			EndDate = endDate;
			IsCurrent = isCurrent;
			Description = description ?? string.Empty;
		}

		// ========= Behaviors =========
		public void Update(
			string jobTitle,
			Guid companyId,
			EmploymentType employmentType,
			DateTime startDate,
			DateTime? endDate,
			bool isCurrent,
			string description)
		{
			CheckRule(new NotEmptyRule(jobTitle, nameof(JobTitle)));
			CheckRule(new ExperienceDateRangeRule(startDate, endDate, isCurrent));

			JobTitle = jobTitle;
			CompanyId = companyId;
			EmploymentType = employmentType;
			StartDate = startDate;
			EndDate = endDate;
			IsCurrent = isCurrent;
			Description = description ?? Description;
		}

		public void MarkAsCurrent()
		{
			IsCurrent = true;
			EndDate = null;
		}

		public void EndExperience(DateTime endDate)
		{
			CheckRule(new ExperienceEndDateMustBeAfterStartDateRule(StartDate, endDate));

			IsCurrent = false;
			EndDate = endDate;
		}
	}
}