using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Enums;
using Jobs.Domain.ValueObjects;


namespace Jobs.Application.Features.Jobs.Commands.UpdateJob
{
	public record UpdateJobCommand(
	string CompanyId,
	string JobId,
	string Title,
	string Description,
	string Requirements,
	EmploymentType EmploymentType,
	int ExperienceLevel,
	decimal? minSalary,
	decimal? maxSalary,
	DateTime? ExpirationDate) : ICommand;


	public record UpdateJobRequest(
	string JobId,
	string Title,
	string Description,
	string Requirements,
	EmploymentType EmploymentType,
	int ExperienceLevel,
	decimal? minSalary,
	decimal? maxSalary,
	DateTime? ExpirationDate) : ICommand;
}
