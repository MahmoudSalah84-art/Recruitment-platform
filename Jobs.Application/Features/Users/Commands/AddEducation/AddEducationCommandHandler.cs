using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace Jobs.Application.Features.Users.Commands.AddEducation
{
	public class AddEducationCommandHandler : IRequestHandler<AddEducationCommand, Result>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public AddEducationCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result> Handle(AddEducationCommand request, CancellationToken cancellationToken)
		{
			var userId = _httpContext.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null) return Result.Failure("Unauthorized");

			var education = new UserEducation
			{
				UserId = Guid.Parse(userId),
				Degree = request.Degree,
				Institution = request.Institution,
				FieldOfStudy = request.FieldOfStudy,
				StartDate = request.StartDate,
				EndDate = request.EndDate
			};

			await _unitOfWork.Educations.AddAsync(education);
			await _unitOfWork.CompleteAsync(); // الحفظ عبر Unit of Work

			return Result.Success();
		}
	}
}