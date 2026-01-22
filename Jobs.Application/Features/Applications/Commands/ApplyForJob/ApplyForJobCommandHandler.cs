//using Jobs.Application.Abstractions.Messaging;


//namespace Jobs.Application.Features.Applications.Commands.ApplyForJob
//{
//	public class ApplyForJobCommandHandler
//		: ICommandHandler<ApplyForJobCommand, Guid>
//	{
//		private readonly IJobRepository _jobRepository;
//		private readonly IUserRepository _userRepository;
//		private readonly IApplicationRepository _applicationRepository;

//		public ApplyForJobCommandHandler(
//			IJobRepository jobRepository,
//			IUserRepository userRepository,
//			IApplicationRepository applicationRepository)
//		{
//			_jobRepository = jobRepository;
//			_userRepository = userRepository;
//			_applicationRepository = applicationRepository;
//		}

//		public async Task<Guid> Handle(ApplyForJobCommand command, CancellationToken cancellationToken)
//		{
//			var job = await _jobRepository.GetByIdAsync(command.JobId)
//				?? throw new BusinessException("Job not found");

//			var user = await _userRepository.GetByIdAsync(command.UserId)
//				?? throw new BusinessException("User not found");

//			var application = JobApplication.Create(
//				job.Id,
//				user.Id,
//				command.CoverLetter);

//			_applicationRepository.Add(application);

//			return application.Id;
//		}
//	}
//}
