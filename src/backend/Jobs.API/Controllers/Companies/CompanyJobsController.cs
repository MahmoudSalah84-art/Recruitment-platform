using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Jobs.Commands.CreateJob;
using Jobs.Application.Features.Jobs.Queries.GetJobsByCompany;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Companies
{
	public class CompanyJobsController : ApiController
	{
		// GET /api/companiesjobs/{companyId}
		[HttpGet]
		[Authorize(Policy = Permissions.Jobs_View)]
		public async Task<IActionResult> GetJobs(string companyId)
		{
			

			var result = await Sender.Send(new GetJobsByCompanyQuery(companyId));

			return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
		}

		// POST /api/companiesjobs/{companyId}
		[HttpPost]
		[Authorize(Policy = Permissions.Jobs_Create)]
		public async Task<IActionResult> CreateJob(string companyId, CreateJobCommand command)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != companyId) return Unauthorized();

			if (companyId != command.CompanyId)
				return BadRequest("CompanyId mismatch");

			var result = await Sender.Send(command);

			return result.IsSuccess ? CreatedAtAction(nameof(GetJobs), new { companyId }, null) : BadRequest(result.Error);
		}
	}
}
