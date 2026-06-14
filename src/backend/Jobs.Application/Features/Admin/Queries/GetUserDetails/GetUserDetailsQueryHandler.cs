using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Common;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.Admin.Queries.GetUserDetails
{

	public class GetUserDetailsQueryHandler
		: IQueryHandler<GetUserDetailsQuery, UserDetailsResponse>
	{
		private readonly IIdentityService _identityService;
		private readonly IUnitOfWork _unitOfWork;

		public GetUserDetailsQueryHandler(
			IIdentityService identityService,
			IUnitOfWork unitOfWork)
		{
			_identityService = identityService;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<UserDetailsResponse>> Handle(
			GetUserDetailsQuery request,
			CancellationToken cancellationToken)
		{

			var jobsPostedCount = 0;
			var applicationsCount = 0;

			var userResult = await _identityService.GetUserByIdAsync(request.UserId);

			if (userResult is null)
				return Result<UserDetailsResponse>.Failure("User not found.");

			if (userResult.Value.Roles.Contains(Roles.Company))
			{
				jobsPostedCount = await _unitOfWork.Jobs.GetCountOfJobsPostedByUserIdAsync(request.UserId);
			}


			if (userResult.Value.Roles.Contains(Roles.User))
			{
				applicationsCount = await _unitOfWork.Applications.GetCountOfApplicationsByUserId(request.UserId);
			}




			return Result<UserDetailsResponse>.Success(new UserDetailsResponse(
				userResult.Value.Id,
				userResult.Value.FirstName,
				userResult.Value.LastName,
				userResult.Value.UserName,
				userResult.Value.Email,
				userResult.Value.Roles,
				userResult.Value.IsActive,
				applicationsCount,
				jobsPostedCount));
		}
	}
}

