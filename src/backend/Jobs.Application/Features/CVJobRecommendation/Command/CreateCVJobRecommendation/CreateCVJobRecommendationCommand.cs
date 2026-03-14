using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation
{
	public record CreateCVJobRecommendationCommand(
	string CvId,
	string JobId,
	int Score) : ICommand<string>;
}
