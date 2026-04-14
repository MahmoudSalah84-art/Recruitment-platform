using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Skills.Commands.DeleteSkill
{
	public class DeleteSkillCommandValidator : AbstractValidator<DeleteSkillCommand>
	{
		public DeleteSkillCommandValidator()
		{
			RuleFor(x => x.SkillId).NotEmpty();
		}
	}
}
