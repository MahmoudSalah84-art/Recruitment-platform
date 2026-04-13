using FluentValidation;

namespace Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation
{
	public class CreateCVJobRecommendationCommandValidator
	: AbstractValidator<CreateCVJobRecommendationCommand>
	{
		public CreateCVJobRecommendationCommandValidator()
		{
			RuleFor(x => x.JobId).NotEmpty().WithMessage("Job ID is required.");
		}
	}
}