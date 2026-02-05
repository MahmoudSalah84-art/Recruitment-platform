using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Skills.Commands.AddUserSkill
{
	public record AddUserSkillCommand(
	Guid SkillId
	) : ICommand;
}
