//using Jobs.Application.Abstractions.Messaging;
//using Jobs.Domain.Entities;
//using Jobs.Domain.IRepository;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Jobs.Application.Features.Companies.Commands.CreateCompany
//{
//	public class CreateCompanyCommandHandler
//		: ICommandHandler<CreateCompanyCommand, Guid>
//	{
//		private readonly ICompanyRepository _companyRepository;

//		public CreateCompanyCommandHandler(ICompanyRepository companyRepository)
//		{
//			_companyRepository = companyRepository;
//		}

//		public async Task<Guid> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
//		{
//			var company = Company.Create(
//				command.Name,
//				command.Industry);

//			_companyRepository.Add(company);

//			return company.Id;
//		}

//        Task<Result<Guid>> IRequestHandler<CreateCompanyCommand, Result<Guid>>.Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
