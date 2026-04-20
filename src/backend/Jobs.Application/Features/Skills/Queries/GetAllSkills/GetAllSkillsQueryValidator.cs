using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Skills.Queries.GetJobsBySkill
{
	public class GetAllSkillsQueryValidator : AbstractValidator<GetAllSkillsQuery>
	{
		public GetAllSkillsQueryValidator()
		{
			
			RuleFor(x => x.Page).GreaterThan(0);
			RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
		}
	}
}
