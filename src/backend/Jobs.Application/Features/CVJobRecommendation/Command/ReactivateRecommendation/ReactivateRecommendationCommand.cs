using Jobs.Application.Abstractions.Messaging;


namespace Jobs.Application.Features.CVJobRecommendation.Command.ReactivateRecommendation
{
	public record ReactivateRecommendationCommand(string RecommendationId) : ICommand;

}
