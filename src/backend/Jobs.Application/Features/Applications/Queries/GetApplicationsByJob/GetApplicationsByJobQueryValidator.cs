using FluentValidation;

namespace Jobs.Application.Features.Applications.Queries.GetApplicationsByJob
{
	public class GetApplicationsByJobQueryValidator : AbstractValidator<GetApplicationsByJobQuery>
	{
		public GetApplicationsByJobQueryValidator()
		{
			RuleFor(x => x.JobId).NotEmpty();
			RuleFor(x => x.Page).GreaterThan(0);
			RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
		}
	}
}
