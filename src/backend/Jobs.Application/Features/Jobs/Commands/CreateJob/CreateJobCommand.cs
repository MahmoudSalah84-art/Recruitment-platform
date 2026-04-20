using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Enums;
using Jobs.Domain.ValueObjects;

namespace Jobs.Application.Features.Jobs.Commands.CreateJob
{
	public record CreateJobCommand(
	string CompanyId,
	string Title,
	string Description,
	string Requirements,
	EmploymentType EmploymentType,
	int ExperienceLevel,
	decimal? minSalary,
	decimal? maxSalary,
	DateTime? ExpirationDate) : ICommand<string>;
}
