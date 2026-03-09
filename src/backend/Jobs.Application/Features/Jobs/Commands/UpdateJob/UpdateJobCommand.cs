using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Enums;
using Jobs.Domain.ValueObjects;


namespace Jobs.Application.Features.Jobs.Commands.UpdateJob
{
	public record UpdateJobCommand(
	string JobId,
	string Title,
	string Description,
	string Requirements,
	EmploymentType EmploymentType,
	int ExperienceLevel,
	SalaryRange? Salary,
	DateTime? ExpirationDate) : ICommand;
}
