using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Skills.Commands.DeleteUserSkill
{
	public record DeleteUserSkillCommand(Guid Id) : ICommand;
}
