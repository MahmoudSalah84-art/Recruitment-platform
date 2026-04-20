using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Skills.Commands.RemoveSkillFromJob
{
	public record RemoveSkillFromJobCommand(
	string JobId,
	string SkillId,
	string CompanyId) : ICommand;

}
