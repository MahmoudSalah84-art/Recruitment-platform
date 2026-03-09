
using FluentValidation;

namespace Jobs.Application.Features.Jobs.Commands.CreateJob
{
	public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
	{
		public CreateJobCommandValidator()
		{
			RuleFor(x => x.CompanyId).NotEmpty().WithMessage("Company ID is required.");
			RuleFor(x => x.HrId).NotEmpty().WithMessage("HR ID is required.");
			RuleFor(x => x.Title).NotEmpty().MaximumLength(100).WithMessage("Title is required and must be under 100 chars.");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
			RuleFor(x => x.Requirements).NotEmpty().WithMessage("Requirements are required.");
			RuleFor(x => x.ExperienceLevel).InclusiveBetween(0, 20).WithMessage("Experience must be between 0 and 20 years.");
			RuleFor(x => x.ExpirationDate).GreaterThan(DateTime.UtcNow).When(x => x.ExpirationDate.HasValue)
				.WithMessage("Expiration date must be in the future.");
		}
	}
}
