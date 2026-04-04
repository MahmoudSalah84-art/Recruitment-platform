using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;

namespace Jobs.Application.Features.CV.Command.CreateOrUpdateResume
{
	public class CreateOrUpdateResumeCommandHandler : ICommandHandler<CreateOrUpdateResumeCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFileService _fileService;

		public CreateOrUpdateResumeCommandHandler(IFileService fileService , IUnitOfWork unitOfWork)
		{
			_fileService = fileService;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(CreateOrUpdateResumeCommand request, CancellationToken cancellationToken)
		{
			//test for AsNoTracking
			var cv = _unitOfWork.CVs.Query().FirstOrDefault(r => r.UserId == request.UserId);

			///*var fileName = $"{Gu*/id.NewGuid()}_{request.File.FileName}";
			//var filePath = Path.Combine("wwwroot/CVs", fileName);

			var FileUrl = await _fileService.UploadFileAsync(request.File);

			if (cv is null)
			{
				cv = new Domain.Entities.CV(
					request.UserId,
					"Title",
					FileUrl,
					"Summary"
				);
				_unitOfWork.CVs.Add(cv);
			}
			else
			{
				cv.UpdateFile(
					FileUrl,
					"Title"
				);
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
	}
}
