using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.UnitOfWork;

namespace Jobs.Application.Features.Users.Queries.GetUserProfile
{
	public class GetUserByIdQueryHandler: IQueryHandler<GetUserByIdQuery, UserResponse?>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
		{
            _unitOfWork = unitOfWork;
		}

		
		public async Task<Result<UserResponse?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			if (!Guid.TryParse(request.UserId, out var userGuid))
				return Result<UserResponse?>.Failure("InvalidUserId");

			var user = await _unitOfWork.Users.GetByIdAsync(userGuid, cancellationToken);

			if (user is null)
				return Result<UserResponse?>.Failure("ToDo");
			

			UserResponse userResponse = new UserResponse
			{
				Id = user.Id,
				FullName = user.FullName,
				Email = user.Email.Value,
				PhoneNumber = user.PhoneNumber?.Value,
				Role = user.Role,
				Bio = user.Bio,
				ProfileImage = user.ProfilePictureUrl,
				Skills = user.Skills,
				CVs = user.CVs,
			};

			return Result<UserResponse?>.Success(userResponse);
		}

        
    }
}