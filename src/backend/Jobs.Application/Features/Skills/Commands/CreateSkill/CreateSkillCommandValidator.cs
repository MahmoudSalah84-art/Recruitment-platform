using FluentValidation;


namespace Jobs.Application.Features.Skills.Commands.CreateSkill
{
	public class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
	{
		public CreateSkillCommandValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Skill name is required.")
				.MaximumLength(50).WithMessage("Skill name must be under 50 characters.");
		}
	}
}
