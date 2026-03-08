using FluentValidation;

namespace Jobs.Application.Features.Companies.Command.LoginCompany
{
	internal class LoginCombanyCommandValidator : AbstractValidator<LoginCombanyCommand>
	{
		public LoginCombanyCommandValidator()
		{
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.Password).NotEmpty();
		}
	}
}

 