using FluentValidation;

namespace Jobs.Application.Features.Companies.Queries.GetAllCompanies
{
	public class GetAllCompaniesQueryValidator : AbstractValidator<GetAllCompaniesQuery>
	{
		public GetAllCompaniesQueryValidator()
		{
			RuleFor(x => x.Page).GreaterThan(0);
			RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
		}
	}
}
