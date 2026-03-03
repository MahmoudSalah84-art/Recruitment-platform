using Jobs.Application.Abstractions.Interfaces;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Infrastructure.Repositories.UnitOfWork;
using MediatR;


namespace Jobs.Application.Features.Companies.Commands.DeleteCompany
{
	public sealed class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Result>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
		{
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result> Handle( DeleteCompanyCommand request,CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId);
			if (company is null)
				return Result.Failure("Company not found");

			_unitOfWork.Companies.Remove(company);
			return Result.Success();
		}
	}

}
