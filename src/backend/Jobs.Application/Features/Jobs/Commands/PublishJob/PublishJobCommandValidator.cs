using FluentValidation;

namespace Jobs.Application.Features.Jobs.Commands.PublishJob
{
	public class PublishJobCommandValidator : AbstractValidator<PublishJobCommand>
	{

		public PublishJobCommandValidator()
		{
			RuleFor(x => x.JobId).NotEmpty();
		}

	}
}
