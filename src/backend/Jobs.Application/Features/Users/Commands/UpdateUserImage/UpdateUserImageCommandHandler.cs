using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Companies.Command.UpdateCompanyLogo;
using Jobs.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Users.Commands.UpdateUserImage
{

	public class UpdateUserImageCommandHandler : ICommandHandler<UpdateUserImageCommand>
	{
		private readonly IFileService _fileService;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateUserImageCommandHandler(
			IFileService fileService,
			IUnitOfWork unitOfWork)
		{
			_fileService = fileService;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(UpdateUserImageCommand request, CancellationToken cancellationToken)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
			if (user is null)
				return Result.Failure("User not found.");
			var imageUrl = await _fileService.UploadImageAsync(request.file);

			user.UpdateProfilePicture(imageUrl);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}