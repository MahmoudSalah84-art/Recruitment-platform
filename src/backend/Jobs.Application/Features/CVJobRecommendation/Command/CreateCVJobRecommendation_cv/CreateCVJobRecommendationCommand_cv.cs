using Jobs.Application.Abstractions.Messaging;

namespace Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation
{
	public sealed record CreateCVJobRecommendationCommand_cv(string UserId) : ICommand;
}
