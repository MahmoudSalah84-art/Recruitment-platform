using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Jobs.Application.Features.Applications.Commands.ApplyForJob
{
	public class ApplyForJobCommandHandler: ICommandHandler<ApplyForJobCommand, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAcc;
		public ApplyForJobCommandHandler(IUnitOfWork unitOfWork , IHttpContextAccessor httpContextAcc)
		{
			_unitOfWork = unitOfWork;
			_httpContextAcc = httpContextAcc;
		}

		public async Task<Result<Guid>> Handle(ApplyForJobCommand command, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContextAcc.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

			var job = await _unitOfWork.Jobs.GetByIdAsync(command.JobId);
			if (job is null )
				return Result<Guid>.Failure("Job not found");

			var cv = await _unitOfWork.CVs.GetByIdAsync(command.CvId);
			if (cv is null)
				return Result<Guid>.Failure("cv not found");

			var application = new JobApplication(
				userId,
				job.Id,
				command.CvId
			);
				
			_unitOfWork.Applications.Add(application);

			return Result<Guid>.Success(application.Id);
		}
    }
}
