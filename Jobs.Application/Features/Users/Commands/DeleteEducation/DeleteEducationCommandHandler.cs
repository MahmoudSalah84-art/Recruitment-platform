using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.DeleteEducation
{
	public class DeleteEducationCommandHandler : IRequestHandler<DeleteEducationCommand, Result>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContext;

		public DeleteEducationCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
		{
			_unitOfWork = unitOfWork;
			_httpContext = httpContext;
		}

		public async Task<Result> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(_httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			var education = await _unitOfWork.Educations.GetByIdAsync(request.Id, cancellationToken);

			if (education == null || education.UserId != userId)
				return Result.Failure("Education record not found.");

			_unitOfWork.Educations.Remove(education);
			await _unitOfWork.CompleteAsync();

			return Result.Success();
		}
	}
}
