using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.CVJobRecommendation.Command.DeactivateRecommendation
{
	public class DeactivateRecommendationCommandValidator
	: AbstractValidator<DeactivateRecommendationCommand>
	{
		public DeactivateRecommendationCommandValidator()
		{
			RuleFor(x => x.RecommendationId).NotEmpty();
		}
	}
}
