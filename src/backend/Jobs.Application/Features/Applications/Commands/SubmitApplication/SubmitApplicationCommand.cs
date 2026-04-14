using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.Applications.Commands.SubmitApplication
{
	public record SubmitApplicationCommand(
	string ApplicantId,
	string JobId,
	string? CvId,
	int MatchScore) : ICommand<string>;
}
