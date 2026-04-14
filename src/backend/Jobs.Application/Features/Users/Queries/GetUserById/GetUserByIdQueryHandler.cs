using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Users.Queries.GetUserProfile
{
	public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse?>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		public async Task<Result<UserResponse?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{

			var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

			if (user is null)
				return Result<UserResponse?>.Failure("user isn't here");


			var userResponse = new UserResponse
			{
				Id = user.Id,
				FirsName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email.Value,
				PhoneNumber = user.PhoneNumber?.Value,
				Bio = user.Bio,
				ProfileImage = user.ProfilePictureUrl,
				Skills = user.Skills,
			
			};

			return Result<UserResponse?>.Success(userResponse);
		}

	}
}