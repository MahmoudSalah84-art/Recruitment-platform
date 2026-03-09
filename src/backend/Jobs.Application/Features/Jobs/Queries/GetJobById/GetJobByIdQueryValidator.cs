using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Jobs.Queries.GetJobById
{
	public class GetJobByIdQueryValidator : AbstractValidator<GetJobByIdQuery>
	{
		public GetJobByIdQueryValidator()
		{
			RuleFor(x => x.JobId).NotEmpty();
		}
	}
}
