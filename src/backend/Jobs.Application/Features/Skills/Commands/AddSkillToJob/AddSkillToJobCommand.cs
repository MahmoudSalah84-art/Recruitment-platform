
using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Skills.Commands.AddSkillToJob
{
	public record AddSkillToJobCommand(
	string JobId,
	string SkillId) : ICommand;
}
