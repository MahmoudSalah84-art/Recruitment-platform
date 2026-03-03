using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
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

			if (request.File == null || request.File.Length == 0)
				return Result.Failure("Please upload a valid CV file");

			var cv = await _unitOfWork.CVs.Query()
				.FirstOrDefaultAsync(r => r.UserId == _currentUser.UserId, cancellationToken);

			var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";
			var filePath = Path.Combine("wwwroot/CVs", fileName);


			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await request.File.CopyToAsync(stream);
			}

			if (cv is null)
			{
				cv = new  Domain.Entities.CV(
					_currentUser.UserId,
					request.Title,
					filePath,
					request.Summary
				);
				_unitOfWork.CVs.Add(cv);
			}
			else
			{
				cv.Update(
					request.Title,
					filePath,
					request.Summary
				);
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
	}
}
