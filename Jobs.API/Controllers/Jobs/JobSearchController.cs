using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Jobs.Queries.GetJobsWithPagination;
using Jobs.Application.Features.Jobs.Queries.SearchJobs;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Jobs
{
	public class JobSearchController : ApiController
	{
		[HttpGet]
		public async Task<ActionResult<PaginatedList<JobDto>>> GetJobs([FromQuery] JobsWithFiltersSpecificationQuery query)
		{
			var result = await Sender.Send(query);
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}
	}
}
