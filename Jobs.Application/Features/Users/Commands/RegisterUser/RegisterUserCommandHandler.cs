//using Jobs.Application.Abstractions.Identity;
//using Jobs.Application.Abstractions.Messaging;
//using Jobs.Domain.Entities;
//using Jobs.Domain.IRepository;
//using MediatR;

//namespace Jobs.Application.Features.Users.Commands.RegisterUser
//{
//	public class RegisterUserCommandHandler
//		: ICommandHandler<RegisterUserCommand, Guid>
//	{
//		private readonly IIdentityService _identityService;
//		private readonly IUserRepository _userRepository;

//		public RegisterUserCommandHandler(
//			IIdentityService identityService,
//			IUserRepository userRepository)
//		{
//			_identityService = identityService;
//			_userRepository = userRepository;
//		}

//		public async Task<Guid> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
//		{
//			// 1️⃣ Create identity user
//			var identityId = await _identityService.CreateUserAsync(
//				command.Email,
//				command.Password,
//				command.FullName);

//			// 2️⃣ Create domain user
//			var user = User.Create(
//				command.FullName,
//				command.Email,
//				identityId);

//			_userRepository.Add(user);

//			return user.Id;
//		}

//        Task<Result<Guid>> IRequestHandler<RegisterUserCommand, Result<Guid>>.Handle(RegisterUserCommand request, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
