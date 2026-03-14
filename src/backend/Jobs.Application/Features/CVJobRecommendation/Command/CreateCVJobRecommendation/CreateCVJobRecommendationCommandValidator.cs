using FluentValidation;

namespace Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation
{
	public class CreateCVJobRecommendationCommandValidator
	: AbstractValidator<CreateCVJobRecommendationCommand>
	{
		public CreateCVJobRecommendationCommandValidator()
		{
			RuleFor(x => x.CvId).NotEmpty().WithMessage("CV ID is required.");
			RuleFor(x => x.JobId).NotEmpty().WithMessage("Job ID is required.");
			RuleFor(x => x.Score)
				.InclusiveBetween(0, 100)
				.WithMessage("Score must be between 0 and 100.");
		}
	}
}