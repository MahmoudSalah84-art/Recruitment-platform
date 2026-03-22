
using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Jobs.Commands.CreateJob;
using Jobs.Application.Features.Jobs.Queries.GetJobsByCompany;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Companies
{
	public class CompanyJobsController : ApiController
	{
		// GET /api/companiesjobs/{companyId}
		[HttpGet]
		public async Task<IActionResult> GetJobs(string companyId)
		{
			var result = await Sender.Send(new GetJobsByCompanyQuery(companyId));

			return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
		}

		// POST /api/companiesjobs/{companyId}
		[HttpPost]
		public async Task<IActionResult> CreateJob(string companyId, CreateJobCommand command)
		{
			if (companyId != command.CompanyId)
				return BadRequest("CompanyId mismatch");

			var result = await Sender.Send(command);

			return result.IsSuccess ? CreatedAtAction(nameof(GetJobs), new { companyId }, null) : BadRequest(result.Error);
		}
	}
}
