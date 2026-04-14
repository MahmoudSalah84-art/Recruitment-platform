using FluentValidation;
using Jobs.Application.Features.Applications.Commands.AcceptApplication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Applications.Commands.RejectApplication
{

	public class RejectApplicationCommandValidator : AbstractValidator<AcceptApplicationCommand>
	{
		public RejectApplicationCommandValidator()
		{
			RuleFor(x => x.ApplicationId).NotEmpty();
		}
	}
}
