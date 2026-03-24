using FluentValidation;
using Jobs.Application.Features.Applications.Queries.GetMyApplications;

namespace Jobs.Application.Features.Applications.Queries.GetUserApplications
{
	public class GetUserApplicationsQueryValidator : AbstractValidator<GetUserApplicationsQuery>
	{
		public GetUserApplicationsQueryValidator()
		{
			RuleFor(x => x.ApplicantId).NotEmpty();
			RuleFor(x => x.PageNumber).GreaterThan(0);
			RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
		}
	}
}
