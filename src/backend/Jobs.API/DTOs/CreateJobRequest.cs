using Jobs.Domain.Enums;

namespace Jobs.API.DTOs
{
	public record CreateJobRequest(
	string Title,
	string Description,
	string Requirements,
	EmploymentType EmploymentType,
	int ExperienceLevel,
	decimal? minSalary,
	decimal? maxSalary,
	DateTime? ExpirationDate);
}
