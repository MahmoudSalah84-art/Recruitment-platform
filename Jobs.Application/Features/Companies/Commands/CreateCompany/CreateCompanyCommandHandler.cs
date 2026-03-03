using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Entities;
using Jobs.Infrastructure.Repositories.UnitOfWork;

namespace Jobs.Application.Features.Companies.Commands.CreateCompany
{
	public sealed class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;

		public CreateCompanyCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<Guid>> Handle( CreateCompanyCommand request, CancellationToken cancellationToken)
		{
			var company = new Company(
				request.Name,
				request.description,
				request.industry,
				request.logoUrl,

				request.Country,
				request.City,
				request.Street,
				request.BuildingNumber,
				request.PostalCode
			);

			await _unitOfWork.Companies.AddAsync(company);

			return Result<Guid>.Success(company.Id);
		}
	}
}
