using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Skills.Queries.GetSkillById
{
	public class GetSkillByIdQueryValidator : AbstractValidator<GetSkillByIdQuery>
	{
		public GetSkillByIdQueryValidator()
		{
			RuleFor(x => x.SkillId).NotEmpty();
		}
	}
}
