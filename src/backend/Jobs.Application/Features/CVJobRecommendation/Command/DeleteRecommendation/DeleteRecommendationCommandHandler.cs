using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.CVJobRecommendation.Command.DeleteRecommendation
{
	public class DeleteRecommendationCommandHandler : ICommandHandler<DeleteRecommendationCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteRecommendationCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle( DeleteRecommendationCommand request, CancellationToken cancellationToken)
		{
			var recommendation = await _unitOfWork.CVJobRecommendations
				.GetByIdAsync(request.RecommendationId, cancellationToken);

			if (recommendation is null)
				return Result.Failure( "Recommendation not found.");

			_unitOfWork.CVJobRecommendations.Remove(recommendation);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
