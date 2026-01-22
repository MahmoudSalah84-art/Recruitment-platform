using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.AddUserSkill
{
	public record AddSkillCommand : IRequest<Result>
	{
		public required string SkillName { get; set; }
	}
}
