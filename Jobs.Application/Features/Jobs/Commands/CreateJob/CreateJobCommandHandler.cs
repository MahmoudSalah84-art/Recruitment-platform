//using Jobs.Application.Abstractions.Messaging;
//using Jobs.Domain.Entities;
//using Jobs.Infrastructure.Repositories.IRepository;
//using Jobs.Infrastructure.Repositories.UnitOfWork;


//namespace Jobs.Application.Features.Jobs.Commands.CreateJob
//{
//	public class CreateJobCommandHandler
//		: ICommandHandler<CreateJobCommand, Guid>
//	{
//		private readonly IRepository<Job> _jobRepo;
//		private readonly IUnitOfWork _unitOfWork;

//		public CreateJobCommandHandler(IRepository<Job> jobRepository, IUnitOfWork unitOfWork)
//		{
//			_jobRepo = jobRepository;
//			_unitOfWork = unitOfWork;
//		}

//		public async Task<Result<Guid>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
//        {
//			try
//			{
//				var job = new Job(
//				request.companyId,
//				request.hrId,
//				request.title,
//				request.salary,
//				request.description,
//				request.requirements,
//				request.expLevel,
//				request.expirationDate);

//				_jobRepo.Add(job);
//				await _unitOfWork.SaveChangesAsync(cancellationToken);


//				return Result<Guid>.Success(job.Id);
//			}
//			catch (Exception ex)
//			{
//				return Result<Guid>.Failure($"حدث خطأ أثناء حفظ الوظيفة: {ex.Message}");
//			}
//		}
//    }
//}
