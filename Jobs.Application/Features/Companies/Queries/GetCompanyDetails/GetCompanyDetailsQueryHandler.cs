//using Jobs.Application.Abstractions.Messaging;

//namespace Jobs.Application.Features.Companies.Queries.GetCompanyDetails
//{
//	public class GetCompanyDetailsQueryHandler
//		: IQueryHandler<GetCompanyDetailsQuery, CompanyDto>
//	{
//		private readonly ReadDbContext _context;

//		public GetCompanyDetailsQueryHandler(ReadDbContext context)
//		{
//			_context = context;
//		}

//		public async Task<CompanyDto> Handle(GetCompanyDetailsQuery query, CancellationToken cancellationToken)
//		{
//			return await _context.Companies
//				.Where(c => c.Id == query.CompanyId)
//				.Select(c => new CompanyDto(
//					c.Id,
//					c.Name,
//					c.Industry))
//				.FirstOrDefaultAsync(cancellationToken)
//				?? throw new BusinessException("Company not found");
//		}

//        Task<Result<CompanyDto>> MediatR.IRequestHandler<GetCompanyDetailsQuery, Result<CompanyDto>>.Handle(GetCompanyDetailsQuery request, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
