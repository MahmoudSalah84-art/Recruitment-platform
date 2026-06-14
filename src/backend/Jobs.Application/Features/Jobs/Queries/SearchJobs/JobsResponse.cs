using Jobs.Domain.Enums;
using Jobs.Domain.ValueObjects;


namespace Jobs.Application.Features.Jobs.Queries.SearchJobs
{
	public record JobsResponse(
	string CompanyName,
	string Title,
	string Description,
	string Requirements,
	string EmploymentType, // part-time, full-time, contract, etc.
	int ExperienceLevel,
	SalaryRange? Salary,
	bool IsPublished,
	bool IsExpired,
	DateTime? ExpirationDate,
	//List<string> RequiredSkills,
	string CompanyId,
	string HrId);
}