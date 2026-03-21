using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
 
namespace Jobs.Application.Features.Companies.Command.DeleteCompany
{
	public class DeleteCompanyCommandHandler : ICommandHandler<DeleteCompanyCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IIdentityService _identityService;

		public DeleteCompanyCommandHandler(IIdentityService identityService, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_identityService = identityService;
		}

		public async Task<Result> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
		{

			var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);
			if (company is null)
				return Result.Failure( "Company not found.");

			


			var userDto =await _identityService.GetUserByIdAsync(request.CompanyId);
			if (userDto is null)
				return Result.Failure("Company not found.");



			_unitOfWork.Companies.Remove(company);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			await _identityService.DeleteUserAsync(request.CompanyId);


			return Result.Success();
		}
	}
}
