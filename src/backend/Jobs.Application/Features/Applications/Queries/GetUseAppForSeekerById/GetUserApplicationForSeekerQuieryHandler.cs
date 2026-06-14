using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Applications.Queries.GetUseAppForSeekerById
{


	public class GetUserApplicationForSeekerQuieryHandler : IQueryHandler<GetUserApplicationForSeekerQuiery, GetUserApplicationForSeekerDTO>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IIdentityService _identityService;
		public GetUserApplicationForSeekerQuieryHandler(IUnitOfWork unitOfWork, IIdentityService identityService)
		{
			_unitOfWork = unitOfWork;
			_identityService = identityService;
		}

		public async Task<Result<GetUserApplicationForSeekerDTO>> Handle(GetUserApplicationForSeekerQuiery request, CancellationToken cancellationToken)
		{

			var application = await _unitOfWork.Applications.GetApplicationWithJobTitleAsync(request.ApplicationId, cancellationToken);
			if (application is null)
				return Result<GetUserApplicationForSeekerDTO>.Failure("Application not found.");
			if (application.ApplicantId != request.userId)
				return Result<GetUserApplicationForSeekerDTO>.Failure("Unauthorized access.");

			var _user = await _identityService.GetUserByIdAsync(application.ApplicantId);

			var dto = new GetUserApplicationForSeekerDTO
				(
					Id: application.Id,
					JobId: application.JobId,
					JobTitle: application.Job.Title,
					MatchScore: application.MatchScore,
					Status: application.Status.ToString()
				);



			return Result<GetUserApplicationForSeekerDTO>.Success(dto);
		}
	}
}
