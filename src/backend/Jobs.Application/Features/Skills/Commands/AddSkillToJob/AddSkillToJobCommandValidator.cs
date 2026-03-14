using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Skills.Commands.AddSkillToJob
{
	public class AddSkillToJobCommandValidator : AbstractValidator<AddSkillToJobCommand>
	{
		public AddSkillToJobCommandValidator()
		{
			RuleFor(x => x.JobId).NotEmpty().WithMessage("Job ID is required.");
			RuleFor(x => x.SkillId).NotEmpty().WithMessage("Skill ID is required.");
		}
	}
}
