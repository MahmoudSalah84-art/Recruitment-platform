using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Identity.Command.GenerateEmailConfirmationToken
{
	public class GenerateEmailConfirmationTokenCommandValidator : AbstractValidator<GenerateEmailConfirmationTokenCommand>
	{
		public GenerateEmailConfirmationTokenCommandValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress();
		}
	}
}
