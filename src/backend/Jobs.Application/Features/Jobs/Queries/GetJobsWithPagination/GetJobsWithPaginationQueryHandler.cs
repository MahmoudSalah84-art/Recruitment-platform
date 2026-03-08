//using Jobs.Application.Abstractions.Messaging;
//using Jobs.Application.Common.Models;
//using Jobs.Infrastructure.Repositories.UnitOfWork;

//namespace Jobs.Application.Features.Jobs.Queries.GetJobsWithPagination
//{
//	public class GetJobsWithPaginationQueryHandler : IQueryHandler<GetJobsWithPaginationQuery, PaginatedList<JobDto>>
//	{
//		private readonly IUnitOfWork _unitOfWork;

//		public GetJobsWithPaginationQueryHandler(IUnitOfWork unitOfWork)
//		{
//			_unitOfWork = unitOfWork;
//		}

//		public async Task<Result<PaginatedList<JobDto>>> Handle(GetJobsWithPaginationQuery request, CancellationToken cancellationToken)
//		{
//			var source = _unitOfWork.Jobs
//				.Query()
//				.OrderBy(x => x.Title)
//				.Select(job => new JobDto
//				{
//					Id = job.Id,
//					Title = job.Title,
//					Description = job.Description,
//					CompanyName = job.Company.Name,
//					JobType = job.EmploymentType,
//					MinSalary = job.Salary!.Min,
//					MaxSalary = job.Salary!.Max,
//					CreatedDate = job.CreatedAt,
//					IsExpired = job.IsExpired,
//					Location = job.Company.CompanyAddress!.Country + ", " + job.Company.CompanyAddress!.City
//				});

//			var paginatedList = await PaginatedList<JobDto>.CreateAsync(source, request.PageNumber, request.PageSize);
//			return Result<PaginatedList<JobDto>>.Success(paginatedList);
//		}
//    }
//}
