using FluentValidation;

namespace Jobs.Application.Features.Companies.Command.UpdateCompany
{
	public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
	{
		public UpdateCompanyCommandValidator()
		{
			RuleFor(x => x.CompanyId).NotEmpty();
			RuleFor(x => x.Industry).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Street).NotEmpty();
			RuleFor(x => x.City).NotEmpty();
			RuleFor(x => x.Country).NotEmpty();
		}
	}
}
