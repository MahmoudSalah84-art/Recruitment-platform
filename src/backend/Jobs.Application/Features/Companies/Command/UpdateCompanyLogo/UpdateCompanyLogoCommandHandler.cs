using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;


namespace Jobs.Application.Features.Companies.Command.UpdateCompanyLogo
{
	public class UpdateCompanyLogoCommandHandler : ICommandHandler<UpdateCompanyLogoCommand >
	{
		private readonly IFileService _fileService;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateCompanyLogoCommandHandler(
			IFileService fileService,
			IUnitOfWork unitOfWork)
		{
			_fileService = fileService;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(UpdateCompanyLogoCommand request, CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);
			if (company is null)
				return Result.Failure( "Company not found.");

			var LogoUrl = await _fileService .UploadImageAsync(request.file);

			company.UpdateLogo(LogoUrl);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
