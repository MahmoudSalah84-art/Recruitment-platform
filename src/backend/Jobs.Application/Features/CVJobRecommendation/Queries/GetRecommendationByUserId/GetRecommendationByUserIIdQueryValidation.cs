using FluentValidation;
using Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationsByCv;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationByUserId
{
	public class GetRecommendationByUserIIdQueryValidation : AbstractValidator<GetRecommendationByUserIdQuery>
	{
		public GetRecommendationByUserIIdQueryValidation()
		{
			RuleFor(x => x.UserId).NotEmpty();
			RuleFor(x => x.Page).GreaterThan(0);
			RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
		}
	}
}
