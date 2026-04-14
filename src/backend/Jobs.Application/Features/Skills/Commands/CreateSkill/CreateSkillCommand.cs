using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Skills.Commands.CreateSkill
{
	public record CreateSkillCommand(string Name) : ICommand<string>;
}
