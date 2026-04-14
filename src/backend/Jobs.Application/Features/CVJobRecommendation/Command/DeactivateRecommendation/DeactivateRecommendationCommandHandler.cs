using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.CVJobRecommendation.Command.DeactivateRecommendation
{
	public class DeactivateRecommendationCommandHandler : ICommandHandler<DeactivateRecommendationCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeactivateRecommendationCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle( DeactivateRecommendationCommand request, CancellationToken cancellationToken)
		{
			var recommendation = await _unitOfWork.CVJobRecommendations
				.GetByIdAsync(request.RecommendationId, cancellationToken);

			if (recommendation is null)
				return Result.Failure( "Recommendation not found.");

			recommendation.Deactivate();

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
