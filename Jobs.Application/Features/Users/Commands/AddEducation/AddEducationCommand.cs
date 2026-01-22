using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.AddEducation
{
	public record AddEducationCommand : IRequest<Result>
	{
		public required string Degree { get; set; }
		public required string Institution { get; set; }
		public required string FieldOfStudy { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
}
