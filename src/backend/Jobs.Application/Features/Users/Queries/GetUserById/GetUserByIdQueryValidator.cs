using FluentValidation;
using Jobs.Application.Features.Users.Queries.GetUserProfile;

namespace Jobs.Application.Features.Users.Queries.GetUserById
{


	public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
	{
		public GetUserByIdQueryValidator()
		{

			RuleFor(x => x.UserId).NotEmpty();

		}
	}
}
