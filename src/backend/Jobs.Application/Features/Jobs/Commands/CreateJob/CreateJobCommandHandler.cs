using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Domain.IRepositories;


namespace Jobs.Application.Features.Jobs.Commands.CreateJob
{
	public class CreateJobCommandHandler : ICommandHandler<CreateJobCommand, string>
	{
		private readonly IRepository<Job> _jobRepo;
		private readonly IUnitOfWork _unitOfWork;

		public CreateJobCommandHandler(IRepository<Job> jobRepository, IUnitOfWork unitOfWork)
		{
			_jobRepo = jobRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<string>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
		{
			var job = new Job(
			request.CompanyId,
			request.HrId, 
			request.Title,
			request.Salary,
			request.Description,
			request.Requirements,
			request.EmploymentType,
			request.ExperienceLevel,
			request.ExpirationDate);

			_unitOfWork.Jobs.Add(job);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<string>.Success(job.Id);
		}
	}
}