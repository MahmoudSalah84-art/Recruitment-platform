using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Companies.Command.AddEmployee
{
	public class AddEmployeeCommandValidator : AbstractValidator<AddEmployeeCommand>
	{
		public AddEmployeeCommandValidator()
		{
			RuleFor(x => x.CompanyId).NotEmpty();
			RuleFor(x => x.EmployeeId).NotEmpty();
		}
	}
}
