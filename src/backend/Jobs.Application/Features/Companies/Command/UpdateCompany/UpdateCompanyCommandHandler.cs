using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.IRepositories;
using Jobs.Domain.ValueObjects.YourProject.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jobs.Application.Features.Companies.Command.UpdateCompany
{
	public class UpdateCompanyCommandHandler : ICommandHandler<UpdateCompanyCommand>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UpdateCompanyCommandHandler( IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
		{
			var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId, cancellationToken);
			if (company is null)
				return Result.Failure("Company not found.");

			var address = Address.Create( request.Country, request.City,request.Street, request.BuildingNumber , request.PostalCode);
			
			company.UpdateProfile(
				request.Description,
				request.Industry,
				request.EmployeesCount,
				address
			);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
