using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.CVJobRecommendation.Command.ReactivateRecommendation
{
	public class ReactivateRecommendationCommandHandler : ICommandHandler<ReactivateRecommendationCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public ReactivateRecommendationCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle( ReactivateRecommendationCommand request, CancellationToken cancellationToken)
		{
			var recommendation = await _unitOfWork.CVJobRecommendations
				.GetByIdAsync(request.RecommendationId, cancellationToken);

			if (recommendation is null)
				return Result.Failure( "Recommendation not found.");

			recommendation.Reactivate();

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}

}
