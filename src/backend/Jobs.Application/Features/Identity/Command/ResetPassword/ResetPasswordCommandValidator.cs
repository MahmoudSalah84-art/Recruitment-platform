
using FluentValidation;
using Jobs.Application.Features.Identity.Command.Login;

namespace Jobs.Application.Features.Identity.Command.ResetPassword
{
	public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
	{

		public ResetPasswordCommandValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(x => x.Token)
				.NotEmpty();

			RuleFor(x => x.NewPassword)
				.NotEmpty()
				.MinimumLength(8)
				.Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
				.Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
				.Matches(@"\d").WithMessage("Password must contain at least one digit.")
				.Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
		}
	}
	
}
