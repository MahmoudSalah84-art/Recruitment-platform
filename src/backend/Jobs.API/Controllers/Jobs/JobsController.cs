using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Abstractions.Messaging;
using Jobs.Application.Features.Jobs.Commands.CreateJob;
using Jobs.Application.Features.Jobs.Commands.DeleteJob;
using Jobs.Application.Features.Jobs.Commands.PublishJob;
using Jobs.Application.Features.Jobs.Commands.UnpublishJob;
using Jobs.Application.Features.Jobs.Commands.UpdateJob;
using Jobs.Application.Features.Jobs.Queries.GetJobById;
using Jobs.Application.Features.Jobs.Queries.SearchJobs;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Jobs
{
	public class JobsController : ApiController
	{
		// GET: api/jobs/search?title=Developer&location=New%20York
		[HttpGet("search")]

		public async Task<IActionResult> SearchJobs([FromQuery] JobsWithFiltersSpecificationQuery query)
		{
			var result = await Sender.Send(query);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		// GET: api/jobs/{id}
		[HttpGet("{id}", Name = "GetJobById")]
		public async Task<IActionResult> GetJobById(string id)
		{
			var result = await Sender.Send(new GetJobByIdQuery(id));

			if (result is null)
				return NotFound();

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
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

			return result.IsSuccess ? CreatedAtRoute( "GetJobById", new { id = result.Value }, result.Value  ) : BadRequest(result.Error);
		}


		// DELETE: api/jobs/{id}
		[HttpDelete("{id}")]
		[Authorize(Policy = Permissions.Jobs_Delete)]
		public async Task<IActionResult> DeleteJob(string id)
		{
			var result = await Sender.Send(new DeleteJobCommand(id));

			if (!result.IsSuccess)
				return NotFound(result.Error);

			return NoContent();
		}

		// POST: http://localhost:5077//api/jobs/{id}/publish
		[HttpPost("{id}/publish")]
		public async Task<IActionResult> PublishJob(string id)
		{
			await Sender.Send(new PublishJobCommand(id));

			return NoContent(); // 204
		}

		// POST: api/jobs/{id}/unpublish
		[HttpPost("{id}/unpublish")]
		public async Task<IActionResult> UnpublishJob(string id)
		{
			await Sender.Send(new UnpublishJobCommand(id));

			return NoContent(); // 204
		}


		// PUT: api/jobs/{id}
		[HttpPut("{id}")]
		[Authorize(Policy = Permissions.Jobs_Update)]
		public async Task<IActionResult> UpdateJob(string id, [FromBody] UpdateJobCommand command)
		{
			if (id != command.JobId)
				return BadRequest("Route id and body id must match");

			await Sender.Send(command);

			return NoContent(); // 204
		}
	}
}