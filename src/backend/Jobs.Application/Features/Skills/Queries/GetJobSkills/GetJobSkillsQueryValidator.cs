using FluentValidation;

namespace Jobs.Application.Features.Skills.Queries.GetSkillsByJob
{
	public class GetJobSkillsQueryValidator : AbstractValidator<GetJobSkillsQuery>
	{
		public GetJobSkillsQueryValidator()
		{
			RuleFor(x => x.JobId).NotEmpty();
		}
	}
}
