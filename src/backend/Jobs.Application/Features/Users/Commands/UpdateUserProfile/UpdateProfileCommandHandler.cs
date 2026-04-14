//using Jobs.Application.Abstractions.Messaging;
//using Jobs.Infrastructure.Repositories.UnitOfWork;
//using Microsoft.AspNetCore.Http;
//using System.Security.Claims;

//namespace Jobs.Application.Features.Users.Commands.UpdateUserProfile
//{
//	public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand>
//	{
//		private readonly IUnitOfWork _unitOfWork;
//		private readonly IHttpContextAccessor _userContext;

//		public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor userContext)
//		{
//			_unitOfWork = unitOfWork;
//			_userContext = userContext;
//		}

//		public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
//		{
//			var userId = _userContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
			
//			if (!Guid.TryParse(userId, out var userGuid))
//				return Result.Failure("InvalidUserId");

//			var user = await _unitOfWork.Users.GetByIdAsync(userGuid, cancellationToken);

//			if (user is null) return Result.Failure("NotFound");

//			user.UpdateProfile(
//				request.FullName,
//				request.Email,
//				request.PhoneNumber!,
//				request.Bio!,
//				request.ProfilePictureUrl!
//			);

//			_unitOfWork.Users.Update(user);
//			await _unitOfWork.SaveChangesAsync(cancellationToken);

//			return Result.Success();
//		}
//    }
//}
