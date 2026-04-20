using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Jobs.Queries.GetJobById;
using Jobs.Application.Features.Skills.Queries.GetSkillsByJob;
using Jobs.Domain.IRepositories;
using Jobs.Domain.Specifications.Skills;

namespace Jobs.Application.Features.Skills.Queries.GetJobsBySkill
{
	public class GetAllSkillsQueryHandler : IQueryHandler<GetAllSkillsQuery, PaginatedList<JobSkillResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetAllSkillsQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<PaginatedList<JobSkillResponse>>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
		{
			var spec = new SearchSkillsSpecification(
				request.Search, request.Page, request.PageSize);

			var Count = await _unitOfWork.Skills.CountAsync(spec);

			var skills = await _unitOfWork.Skills.ListWithSpecAsync(spec);


			var response = skills
				.Select(j => new JobSkillResponse(
					j.Id,
					j.Name)
				).ToList();
			var paginatedList = new PaginatedList<JobSkillResponse>(response, Count, request.Page, request.PageSize);

			return Result<PaginatedList<JobSkillResponse>>.Success(paginatedList);
		}
	}
}