//using Jobs.Application.Abstractions.Messaging;


//namespace Jobs.Application.Features.Jobs.Queries.SearchJobs
//{
//	public class SearchJobsQueryHandler
//		: IQueryHandler<SearchJobsQuery, IReadOnlyList<JobDto>>
//	{
//		private readonly ReadDbContext _context;

//		public SearchJobsQueryHandler(ReadDbContext context)
//		{
//			_context = context;
//		}

//		public async Task<IReadOnlyList<JobDto>> Handle(SearchJobsQuery query, CancellationToken cancellationToken)
//		{
//			return await _context.JobSearch
//				.Where(j => j.Title.Contains(query.Keyword))
//				.Select(j => new JobDto(
//					j.JobId,
//					j.Title,
//					j.CompanyName))
//				.ToListAsync(cancellationToken);
//		}

//        Task<Result<IReadOnlyList<JobDto>>> MediatR.IRequestHandler<SearchJobsQuery, Result<IReadOnlyList<JobDto>>>.Handle(SearchJobsQuery request, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
