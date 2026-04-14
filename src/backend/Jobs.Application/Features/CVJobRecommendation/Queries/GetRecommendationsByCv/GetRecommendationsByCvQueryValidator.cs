using FluentValidation;

namespace Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationsByCv
{
	public class GetRecommendationsByCvQueryValidator : AbstractValidator<GetRecommendationsByCvQuery>
	{
		public GetRecommendationsByCvQueryValidator()
		{
			RuleFor(x => x.CvId).NotEmpty();
			RuleFor(x => x.Page).GreaterThan(0);
			RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
		}
	}
}
