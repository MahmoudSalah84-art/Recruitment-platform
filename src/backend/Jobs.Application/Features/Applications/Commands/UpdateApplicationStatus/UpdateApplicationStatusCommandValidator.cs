using FluentValidation;


namespace Jobs.Application.Features.Applications.Commands.UpdateApplicationStatus
{
	 
	public class UpdateApplicationStatusCommandValidator
	: AbstractValidator<UpdateApplicationStatusCommand>
	{
		public UpdateApplicationStatusCommandValidator()
		{
			RuleFor(x => x.ApplicationId)
				.NotEmpty();

			RuleFor(x => x.NewStatus)
				.IsInEnum()
				.WithMessage("Invalid application status.");
		}
	}
}
