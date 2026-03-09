using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Jobs.Commands.UpdateJob
{
	public class UpdateJobCommandHandler : ICommandHandler<UpdateJobCommand>
	{
		private readonly IJobRepository _jobRepository;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateJobCommandHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork)
		{
			_jobRepository = jobRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
		{
			var job = await _jobRepository.GetByIdAsync(request.JobId, cancellationToken);

			if (job is null)
				return Result.Failure( "Job not found.");

			job.UpdateDetails(
				request.Title,
				request.Description,
				request.Requirements,
				request.EmploymentType,
				request.ExperienceLevel,
				request.Salary,
				request.ExpirationDate);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
