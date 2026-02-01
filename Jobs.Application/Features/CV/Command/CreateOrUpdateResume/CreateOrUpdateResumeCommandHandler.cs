using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Interfaces;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Application.Features.CV.Command.CreateOrUpdateResume
{
	public class CreateOrUpdateResumeCommandHandler : ICommandHandler<CreateOrUpdateResumeCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public CreateOrUpdateResumeCommandHandler(
			IUnitOfWork unitOfWork,
			ICurrentUserService currentUser)
		{
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result> Handle(CreateOrUpdateResumeCommand request, CancellationToken cancellationToken)
		{
			if (_currentUser.UserId == Guid.Empty)
				return Result.Failure("User not authenticated");

			if (string.IsNullOrWhiteSpace(request.Summary))
				return Result.Failure("Summary is required");

			var cv = await _unitOfWork.CVs.Query()
				.FirstOrDefaultAsync(r => r.UserId == _currentUser.UserId, cancellationToken);

			if (cv is null)
			{
				cv = new  Domain.Entities.CV(
					_currentUser.UserId,
					request.Title,
					request.FilePath,
					request.Summary
				);

				_unitOfWork.CVs.Add(cv);
			}
			else
			{
				cv.Update(
					request.Title,
					request.FilePath,
					request.Summary
				);
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
	}
}
