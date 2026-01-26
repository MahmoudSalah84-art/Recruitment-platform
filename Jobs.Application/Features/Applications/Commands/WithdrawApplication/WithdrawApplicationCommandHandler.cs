using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Applications.Commands.WithdrawApplication;
using Jobs.Domain.Enums;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Jobs.Application.Features.Applications.Commands.WithdrawApplication
{
    public class WithdrawApplicationCommandHandler : ICommandHandler<WithdrawApplicationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAcc;

        public WithdrawApplicationCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _httpContextAcc = httpContext;
        }

        public async Task<Result> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpContextAcc.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
			var application = await _unitOfWork.Applications.GetByIdAsync(request.ApplicationId, cancellationToken);

            if (application is null)
                return Result.Failure("Application not found.");

			if (application.ApplicantId != userId)
				return Result<Unit>.Failure("you don't have athourization for this application.");

			if (application.Status != ApplicationStatus.Pending)
                return Result.Failure("Cannot withdraw an application that has already been processed.");

            _unitOfWork.Applications.Remove(application);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}