using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Applications.Queries.GetApplicationById
{
	//public class GetUserApplicationDetailsQuieryHandler : IQueryHandler<GetUserApplicationDetailsQuery, GetUserApplicationDetailsDTO>
	//{
	//	private readonly IUnitOfWork _unitOfWork;

	//	public GetUserApplicationDetailsQuieryHandler(IUnitOfWork unitOfWork)
	//	{
	//		_unitOfWork = unitOfWork;
	//	}

	//	public async Task<Result<GetUserApplicationDetailsDTO>> Handle(GetUserApplicationDetailsQuery query, CancellationToken cancellationToken)
	//	{
	//		var application = await _unitOfWork.Applications.Query()
	//			.Include(a => a.Job)
	//				.ThenInclude(j => j.Company)
	//			.Include(a => a.Applicant)
	//			.FirstOrDefaultAsync(a => a.Id == query.Id, cancellationToken);

	//		if (application is null)
	//			return Result<GetUserApplicationDetailsDTO>.Failure("عذراً، لم يتم العثور على طلب التقديم هذا.");

	//		var dto = new GetUserApplicationDetailsDTO
	//		{
	//			Id = application.Id,
	//			JobId = application.Job.Id,
	//			JobTitle = application.Job.Title,
	//			CompanyName = application.Job.Company.Name,
	//			ApplicantName = application.Applicant.FullName,
	//			Status = application.Status.ToString(),
	//			AppliedAt = application.CreatedAt
	//		};

	//		return Result<GetUserApplicationDetailsDTO>.Success(dto);
	//	}
	//}
}
