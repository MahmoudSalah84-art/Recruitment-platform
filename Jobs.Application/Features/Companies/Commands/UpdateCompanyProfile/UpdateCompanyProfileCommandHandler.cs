using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Interfaces;
using Jobs.Domain.ValueObjects.YourProject.Domain.ValueObjects;
using Jobs.Infrastructure.Repositories.UnitOfWork;

namespace Jobs.Application.Features.Companies.Commands.UpdateCompanyProfile
{
	public class UpdateCompanyProfileCommandHandler : ICommandHandler<UpdateCompanyProfileCommand>
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUser;

		public UpdateCompanyProfileCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
		{
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result> Handle( UpdateCompanyProfileCommand request, CancellationToken cancellationToken)
		{
			if (_currentUser.UserId == Guid.Empty)
				return Result.Failure("User not authenticated");

			var company = await _unitOfWork.Companies
				.GetByIdAsync(_currentUser.UserId, cancellationToken);

			if (company is null)
				return Result.Failure("Company not found");

			var address = Address.Create(
				request.Country,
				request.City,
				request.Street,
				request.BuildingNumber,
				request.PostalCode
			);

			company.UpdateProfile(
				request.Name,
				request.Description,
				request.Industry,
				request.EmployeesCount,
				request.LogoUrl,
				address
			);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
	}


}
