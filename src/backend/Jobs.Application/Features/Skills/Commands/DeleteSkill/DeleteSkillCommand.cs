using FluentValidation;
using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Skills.Commands.DeleteSkill
{
	public record DeleteSkillCommand(string SkillId) : ICommand;

	
}
