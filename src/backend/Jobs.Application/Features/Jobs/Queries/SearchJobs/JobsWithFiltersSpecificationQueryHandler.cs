//using Jobs.Application.Abstractions.Messaging;
//using Jobs.Application.Common.Models;
//using Jobs.Application.Features.Jobs.Queries.GetJobsWithPagination;
//using Jobs.Domain.Specifications.Jobs;
//using Jobs.Infrastructure.Repositories.UnitOfWork;
//using Microsoft.EntityFrameworkCore;

//namespace Jobs.Application.Features.Jobs.Queries.SearchJobs
//{
//	public class JobsWithFiltersSpecificationQueryHandler : IQueryHandler<JobsWithFiltersSpecificationQuery, PaginatedList<JobDto>>
//	{
//		private readonly IUnitOfWork _unitOfWork;

//		public JobsWithFiltersSpecificationQueryHandler(IUnitOfWork unitOfWork)
//		{
//			_unitOfWork = unitOfWork;
//		}

		 
//		public async Task<Result<PaginatedList<JobDto>>> Handle(JobsWithFiltersSpecificationQuery request, CancellationToken cancellationToken)
//		{
//			var spec = new JobsWithFiltersSpecification(request.Search, request.PageSize, request.PageNumber, request.Country, request.City, request.MinExperience, request.Type, request.PostedInDays);

//			var Count = await _unitOfWork.Jobs.CountAsync(spec);

//			var jobs = await _unitOfWork.Jobs.ListWithSpecAsync(spec);

//			var dtos = jobs.Select(job => new JobDto
//			{
//				Id = job.Id,
//				Title = job.Title,
//				Description = job.Description,
//				CompanyName = job.Company.Name,
//				JobType = job.EmploymentType,
//				MinSalary = job.Salary!.Min,
//				MaxSalary = job.Salary!.Max,
//				CreatedDate = job.CreatedAt,
//				IsExpired = job.IsExpired,
//				Location = job.Company.CompanyAddress!.Country + ", " + job.Company.CompanyAddress!.City
//			}).ToList();

//			PaginatedList<JobDto> paginatedList = PaginatedList<JobDto>.Create(dtos, Count, request.PageNumber, request.PageSize);
//			return Result<PaginatedList<JobDto>>.Success(paginatedList);
//		}   
//    }
//}