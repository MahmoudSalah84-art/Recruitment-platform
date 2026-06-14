using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories; 

namespace Jobs.Application.Features.Applications.Queries.GetApplicationById
{
	public class GetUserApplicationDetailsQuieryHandler : IQueryHandler<GetUserApplicationDetailsQuery, GetUserApplicationDetailsDTO>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IIdentityService _identityService;
		public GetUserApplicationDetailsQuieryHandler(IUnitOfWork unitOfWork, IIdentityService identityService)
		{
			_unitOfWork = unitOfWork;
			_identityService = identityService;
		}

		public async Task<Result<GetUserApplicationDetailsDTO>> Handle(GetUserApplicationDetailsQuery request, CancellationToken cancellationToken)
		{

			var application = await _unitOfWork.Applications.GetApplicationWithCVAsync(request.ApplicationId, cancellationToken);
			if (application is null)
				return Result<GetUserApplicationDetailsDTO>.Failure("Application not found.");

			var job = await _unitOfWork.Jobs.GetByIdAsync(application.JobId, cancellationToken);
			if (job is null)
				return Result<GetUserApplicationDetailsDTO>.Failure("job not found.");

			var _user = await _identityService.GetUserByIdAsync(application.ApplicantId);

			var dto = new GetUserApplicationDetailsDTO
				(
					Id: application.Id,
					ApplicantId: application.ApplicantId,
					ApplicantName: _user.Value.UserName,
					Email: _user.Value.Email,
					JobTitle: job.Title,
					CvPath: application.CV.FilePath.Value,
					MatchScore: application.MatchScore,
					Status: application.Status.ToString()
				);



			return Result<GetUserApplicationDetailsDTO>.Success(dto);
		}



		//public async Task<Result<GetUserApplicationDetailsDTO>> Handle(GetUserApplicationDetailsQuery request, CancellationToken cancellationToken)
		//{

		//	var application = await _unitOfWork.Applications.GetApplicationWithCVAsync(request.ApplicationId);

		//	if (application is null)
		//		return Result<GetUserApplicationDetailsDTO>.Failure("Application not found.");

		//	var job = await _unitOfWork.Jobs.GetByIdAsync(application.JobId);
		//	if (job is null)
		//		return Result<GetUserApplicationDetailsDTO>.Failure("job not found.");

		//	//if (job.CompanyId != request.companyId)
		//	//	return Result<GetUserApplicationDetailsDTO>.Failure("Unauthorized access.");

		//	var _user = await _identityService.GetUserByIdAsync(application.ApplicantId);
		//	if (_user is null)
		//		return Result<GetUserApplicationDetailsDTO>.Failure("Applicant not found.");

		//	var dto = new GetUserApplicationDetailsDTO
		//		(
		//			Id: application.Id,
		//			ApplicantId: application.ApplicantId,
		//			ApplicantName: _user.Value.UserName,
		//			Email: _user.Value.Email,
		//			JobTitle: application.Job.Title,
		//			CvPath: application.CV.FilePath.Value,
		//			MatchScore: application.MatchScore,
		//			Status: application.Status.ToString()
		//		);


		//	return Result<GetUserApplicationDetailsDTO>.Success(dto);
		//}
	}
}
