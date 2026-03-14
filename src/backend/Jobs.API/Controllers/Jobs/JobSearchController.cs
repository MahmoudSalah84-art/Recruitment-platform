using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Common.Models;
using Jobs.Application.Features.Jobs.Queries.SearchJobs;
using Jobs.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Jobs
{
	public class JobSearchController : ApiController
	{
		[HttpGet]
		public async Task<ActionResult<PaginatedList<JobsResponse>>> GetJobs([FromQuery] JobsWithFiltersSpecificationQuery query)
		{
			var result = await Sender.Send(query);
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}
	}
}

//GET / api / jobs / search
