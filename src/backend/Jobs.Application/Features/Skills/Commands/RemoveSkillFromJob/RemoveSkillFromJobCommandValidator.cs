using FluentValidation;

namespace Jobs.Application.Features.Skills.Commands.RemoveSkillFromJob
{
	public class RemoveSkillFromJobCommandValidator : AbstractValidator<RemoveSkillFromJobCommand>
	{
		public RemoveSkillFromJobCommandValidator()
		{
			RuleFor(x => x.JobId).NotEmpty();
			RuleFor(x => x.SkillId).NotEmpty();
		}
	}
}
