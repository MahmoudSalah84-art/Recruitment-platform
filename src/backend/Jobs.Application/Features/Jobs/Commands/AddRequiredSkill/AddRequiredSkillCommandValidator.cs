using FluentValidation;


namespace Jobs.Application.Features.Jobs.Commands.AddRequiredSkill
{
	public class AddRequiredSkillCommandValidator : AbstractValidator<AddRequiredSkillCommand>
	{
		public AddRequiredSkillCommandValidator()
		{
			RuleFor(x => x.JobId).NotEmpty();
			RuleFor(x => x.SkillId).NotEmpty();
			RuleFor(x => x.SkillName).NotEmpty();
		}
	}
}
