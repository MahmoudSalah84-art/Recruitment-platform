using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Applications.Commands.RerankJobApplicantsJob
{
	public sealed record RerankJobApplicantsJobCommand(
	string ApplicationId ) : ICommand;
}
