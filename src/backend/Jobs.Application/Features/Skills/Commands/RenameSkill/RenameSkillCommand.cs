using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Skills.Commands.RenameSkill
{
	public record RenameSkillCommand(string SkillId, string NewName) : ICommand;
}
