using FluentValidation;

namespace Jobs.Application.Features.Identity.Command.Register
{
	public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
	{
		public RegisterCommandValidator()
		{
			RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
				.Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
				.Matches("[0-9]").WithMessage("Password must contain a digit.")
				.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain a special character.");
			RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match.");
		}
	}
}
