using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;


namespace Jobs.Application.Features.Companies.Command.UpdateCompanyLogo
{
	public class UpdateCompanyLogoCommandHandler : ICommandHandler<UpdateCompanyLogoCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateCompanyLogoCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(UpdateCompanyLogoCommand request, CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);
			if (company is null)
				return Result.Failure( "Company not found.");

			company.UpdateLogo(request.LogoUrl);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
