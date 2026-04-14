using Jobs.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Jobs.Queries.GetJobById
{
	public record JobResponse(
	string Id,
	string Title,
	string Description,
	string Requirements,
	string EmploymentType,
	int ExperienceLevel,
	SalaryRange? Salary,
	bool IsPublished,
	bool IsExpired,
	DateTime? ExpirationDate,
	List<string> RequiredSkills,
	string CompanyId,
	string HrId);
}
