using FluentValidation;


namespace Jobs.Application.Features.Skills.Commands.RenameSkill
{
	public class RenameSkillCommandValidator : AbstractValidator<RenameSkillCommand>
	{
		public RenameSkillCommandValidator()
		{
			RuleFor(x => x.SkillId).NotEmpty();
			RuleFor(x => x.NewName)
				.NotEmpty().WithMessage("Skill name is required.")
				.MaximumLength(50).WithMessage("Skill name must be under 50 characters.");
		}
	}
}
