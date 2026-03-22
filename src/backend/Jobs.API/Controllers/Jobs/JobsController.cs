using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Jobs.Commands.CreateJob;
using Jobs.Application.Features.Jobs.Commands.DeleteJob;
using Jobs.Application.Features.Jobs.Commands.PublishJob;
using Jobs.Application.Features.Jobs.Commands.UnpublishJob;
using Jobs.Application.Features.Jobs.Commands.UpdateJob;
using Jobs.Application.Features.Jobs.Queries.GetJobById;
using Jobs.Application.Features.Jobs.Queries.SearchJobs;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Jobs
{
    public class JobsController : ApiController
    {
		// GET: api/jobs/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetJobById(string id)
		{
			var result = await Sender.Send(new GetJobByIdQuery(id));

			if (result is null)
				return NotFound();

			return Ok(result);
		}

		// GET: api/jobs/search?title=Developer&location=New%20York
		[HttpGet("search")]
		public async Task<IActionResult> SearchJobs([FromQuery] JobsWithFiltersSpecificationQuery query)
		{
			var result = await Sender.Send(query);

			return Ok(result);
		}

		// POST: api/jobs
		[HttpPost]
		public async Task<IActionResult> CreateJob([FromBody] CreateJobCommand command)
		{
			var jobId = await Sender.Send(command);

			return CreatedAtAction(nameof(GetJobById), new { id = jobId }, jobId);
		}

		// DELETE: api/jobs/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteJob(string id)
		{
			var result = await Sender.Send(new DeleteJobCommand(id));

			if (!result.IsSuccess)
				return NotFound(result.Error);

			return NoContent();
		}

		// POST: api/jobs/{id}/publish
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
		public async Task<IActionResult> UpdateJob(string id, [FromBody] UpdateJobCommand command)
		{
			if (id != command.JobId)
				return BadRequest("Route id and body id must match");

			await Sender.Send(command);

			return NoContent(); // 204
		}
	}
}

//| Method  |		Route					| Description					|
//| ------  | -----------------------		| ---------------------------	|
//| GET		| `/ api / jobs`                | Get all jobs(with filters)	|
//| GET		| `/ api / jobs /{ id}`         | Job details					|
//| POST	| `/ api / jobs`                | Create job(Employedr only)    |
//| PUT		| `/ api / jobs /{ id}`         | Update job					|
//| DELETE	| `/ api / jobs /{ id}`			| Delete job					|
//| GET		| `/ api / jobs / recommended`  | Recommended jobs for user		|