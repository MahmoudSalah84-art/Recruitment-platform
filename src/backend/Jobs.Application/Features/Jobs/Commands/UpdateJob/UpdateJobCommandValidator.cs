using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Jobs.Commands.UpdateJob
{
	public class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
	{
		public UpdateJobCommandValidator()
		{
			RuleFor(x => x.JobId).NotEmpty();
			RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
			RuleFor(x => x.Description).NotEmpty();
			RuleFor(x => x.Requirements).NotEmpty();
			RuleFor(x => x.ExperienceLevel).InclusiveBetween(0, 20);
			RuleFor(x => x.ExpirationDate).GreaterThan(DateTime.UtcNow).When(x => x.ExpirationDate.HasValue);
		}
	}

}
