using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Applications.Commands.ApplyForJob
{
	public record ApplyForJobCommand(Guid JobId ,Guid CvId) : ICommand<Guid>;
}
