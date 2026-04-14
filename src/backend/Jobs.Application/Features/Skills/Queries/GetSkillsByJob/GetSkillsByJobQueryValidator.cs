using FluentValidation;

namespace Jobs.Application.Features.Skills.Queries.GetSkillsByJob
{
	public class GetSkillsByJobQueryValidator : AbstractValidator<GetSkillsByJobQuery>
	{
		public GetSkillsByJobQueryValidator()
		{
			RuleFor(x => x.JobId).NotEmpty();
		}
	}
}
