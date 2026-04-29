using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Applications.Queries.GetApplicationById
{
	public class GetUserApplicationDetailsQuieryHandler : IQueryHandler<GetUserApplicationDetailsQuery, GetUserApplicationDetailsDTO>
	{
		private readonly IUnitOfWork _unitOfWork;
		public GetUserApplicationDetailsQuieryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<GetUserApplicationDetailsDTO>> Handle(GetUserApplicationDetailsQuery request, CancellationToken cancellationToken)
		{
			var dto = _unitOfWork.Applications
			.Query()
			.Where(a => a.Id == request.ApplicationId)
			.Select(a => new GetUserApplicationDetailsDTO
			(
				Id: a.Id,
				ApplicantId: a.ApplicantId,
				ApplicantName: a.Applicant.FirstName + " " + a.Applicant.LastName,
				JobId: a.Job.Id,
				JobTitle: a.Job.Title,
				CvId: a.CvId,
				MatchScore: a.MatchScore,
				Status: a.Status.ToString(),
				CompanyId: a.Job.Company.Id,
				CompanyName: a.Job.Company.Name
			))
			.FirstOrDefault();

			if (dto is null)
				return Result<GetUserApplicationDetailsDTO>.Failure( "Application not found.");
			// Ensure that the requesting user is either the applicant or the company to which the job was posted
			if (dto.ApplicantId != request.CompanyId || dto.CompanyId != request.CompanyId)
				return Result<GetUserApplicationDetailsDTO>.Failure("Unauthorized access.");



			return Result<GetUserApplicationDetailsDTO>.Success(dto);
		}
	}
}
