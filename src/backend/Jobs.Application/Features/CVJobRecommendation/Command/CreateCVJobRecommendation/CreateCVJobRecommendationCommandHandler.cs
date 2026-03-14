using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
using CVJobRecommendationEntity = Jobs.Domain.Entities.CVJobRecommendation;

namespace Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation
{
	internal class CreateCVJobRecommendationCommandHandler : ICommandHandler<CreateCVJobRecommendationCommand, string>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateCVJobRecommendationCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<string>> Handle(CreateCVJobRecommendationCommand request, CancellationToken cancellationToken)
		{
			var cvExists = await _unitOfWork.CVs.ExistsAsync(X => X.Id == request.CvId);
			if (!cvExists)
				return Result<string>.Failure("CV not found.");

			var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId, cancellationToken);
			if (job is null)
				return Result<string>.Failure("Job not found.");

			if (!job.IsPublished)
				return Result<string>.Failure("Cannot recommend an unpublished job.");

			if (job.IsExpired)
				return Result<string>.Failure("Cannot recommend an expired job.");

			var exists = await _unitOfWork.CVJobRecommendations.ExistsByCvAndJobAsync(request.CvId, request.JobId, cancellationToken);

			if (exists)
				return Result<string>.Failure("A recommendation already exists for this CV and Job.");

			var recommendation = new CVJobRecommendationEntity(request.CvId, request.JobId, request.Score);

			_unitOfWork.CVJobRecommendations.Add(recommendation);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<string>.Success(recommendation.Id);
		}
	}
}