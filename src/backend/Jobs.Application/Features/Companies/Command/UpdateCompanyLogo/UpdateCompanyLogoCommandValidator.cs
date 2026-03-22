using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Companies.Command.UpdateCompanyLogo
{
	public class UpdateCompanyLogoCommandValidator : AbstractValidator<UpdateCompanyLogoCommand>
	{
		public UpdateCompanyLogoCommandValidator()
		{
			RuleFor(x => x.CompanyId).NotEmpty();
		}
	}
}
