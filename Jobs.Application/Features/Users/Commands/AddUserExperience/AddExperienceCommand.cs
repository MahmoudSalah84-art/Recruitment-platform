using Jobs.Application.Abstractions.Messaging;
using MediatR;

namespace Jobs.Application.Features.Users.Commands.AddUserExperience
{
	public record AddExperienceCommand : IRequest<Result>
	{
		public required string Title { get; set; }
		public required string Company { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
}
