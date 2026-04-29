using FluentValidation;

namespace Jobs.Application.Features.Applications.Commands.SubmitApplication
{
	public class SubmitApplicationCommandValidator : AbstractValidator<SubmitApplicationCommand>
	{
		public SubmitApplicationCommandValidator()
		{
			RuleFor(x => x.ApplicantId).NotEmpty();
			RuleFor(x => x.JobId).NotEmpty();
		}
	}
}