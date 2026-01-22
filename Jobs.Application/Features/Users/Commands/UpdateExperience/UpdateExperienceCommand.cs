using Jobs.Application.Abstractions.Messaging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.UpdateExperience
{
	public record UpdateExperienceCommand : IRequest<Result>
	{
		public Guid Id { get; set; }
		public required string Title { get; set; }
		public required string Company { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
}
