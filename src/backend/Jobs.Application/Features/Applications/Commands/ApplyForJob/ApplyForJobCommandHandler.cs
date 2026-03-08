using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;
using System.Security.Claims;

namespace Jobs.Application.Features.Applications.Commands.ApplyForJob
{
 //   public class ApplyForJobCommandHandler : ICommandHandler<ApplyForJobCommand, Guid>
	//{
	//	private readonly IUnitOfWork _unitOfWork;
	//	private readonly IHttpContextAccessor _httpContextAcc;

	//	public ApplyForJobCommandHandler(IUnitOfWork unitOfWork , IHttpContextAccessor httpContextAcc)
	//	{
	//		_unitOfWork = unitOfWork;
	//		_httpContextAcc = httpContextAcc;
	//	}

	//	public async Task<Result<Guid>> Handle(ApplyForJobCommand command, CancellationToken cancellationToken)
	//	{
	//		var userId = Guid.Parse(_httpContextAcc.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

	//		var job = await _unitOfWork.Jobs.GetByIdAsync(command.JobId);
	//		if (job is null )
	//			return Result<Guid>.Failure("Job not found");

	//		var user = await _unitOfWork.Users.GetByIdAsync(userId);
	//		if (user!.CV is null)
	//			return Result<Guid>.Failure("You Don't Have CV");

	//		var alreadyApplied = await _unitOfWork.Applications.Query()
	//			.AnyAsync(a => a.JobId == command.JobId && a.ApplicantId == userId, cancellationToken);
	//		if (alreadyApplied)
	//			return Result<Guid>.Failure("Alrready you Applied For This Job.");

	//		var application = new JobApplication(
	//			userId,
	//			job.Id,
	//			user!.CV.Id
	//		);
				
	//		_unitOfWork.Applications.Add(application);



	//		return Result<Guid>.Success(application.Id);
	//	}
 //   }
}
