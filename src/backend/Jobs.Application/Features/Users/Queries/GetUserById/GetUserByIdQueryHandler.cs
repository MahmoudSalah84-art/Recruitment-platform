using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Users.Queries.GetUserProfile
{
	public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse?>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IIdentityService _svc;

		public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IIdentityService svc)
		{
			_unitOfWork = unitOfWork;
			_svc = svc;
		}

		public async Task<Result<UserResponse?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{

			var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

			if (user is null)
				return Result<UserResponse?>.Failure("user isn't here");
			var otherDitelseOfUder =await _svc.GetUserByIdAsync(request.UserId);


			var userResponse = new UserResponse
			{
				Id = user.Id,
				FirsName = user.FirstName,
				LastName = user.LastName,
				Bio = user.Bio,
				ProfileImage = user.ProfilePictureUrl,
				Skills = user.Skills,
				Email = otherDitelseOfUder.Value.Email,
				PhoneNumber = "01078945612"

			};

			return Result<UserResponse?>.Success(userResponse);
		}

	}
}