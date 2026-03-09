using Jobs.Application.Abstractions.Messaging;


namespace Jobs.Application.Features.Jobs.Commands.AddRequiredSkill
{
	public record AddRequiredSkillCommand(string JobId, string SkillId, string SkillName) 
		: ICommand;
}
