using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Enums;
using Jobs.Domain.ValueObjects;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Jobs.Application.Features.Jobs.Commands.CreateJob
{
	public record CreateJobCommand(
	string CompanyId,
	string HrId,
	string Title,
	string Description,
	string Requirements,
	EmploymentType EmploymentType,
	int ExperienceLevel,
	SalaryRange? Salary,
	DateTime? ExpirationDate) : ICommand<string>;
}
