using FluentValidation;

namespace Jobs.Application.Features.Applications.Commands.AcceptApplication
{
	public class AcceptApplicationCommandValidator : AbstractValidator<AcceptApplicationCommand>
	{
		public AcceptApplicationCommandValidator()
		{
			RuleFor(x => x.ApplicationId).NotEmpty();
		}
	}
}
