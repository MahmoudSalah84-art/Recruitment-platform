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
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Jobs
{
	public class JobsController : ApiController
	{
		/// <summary>
		/// Searches and retrieves jobs based on dynamic filters.
		/// </summary>
		/// <param name="query">The filtering and pagination parameters for job search.</param>
		/// <returns>
		/// Returns a paginated list of jobs matching the specified filters.
		/// </returns>
		/// <remarks>
		/// - Uses CQRS pattern via JobsWithFiltersSpecificationQuery.
		/// - Supports dynamic filtering (e.g., title, location, type, etc.).
		/// - Typically used for job discovery and search functionality.
		/// </remarks>
		/// <response code="200">Jobs retrieved successfully.</response>
		/// <response code="400">Invalid query parameters.</response>
		// GET: api/jobs/search?title=Developer&location=New%20York
		[HttpGet("search")]
		public async Task<IActionResult> SearchJobs([FromQuery] JobsWithFiltersSpecificationQuery query)
		{
			var result = await Sender.Send(query);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Retrieves a specific job by its unique identifier.
		/// </summary>
		/// <param name="jobid">The unique identifier of the job.</param>
		/// <returns>
		/// Returns the job details if found.
		/// </returns>
		/// <remarks>
		/// - Uses CQRS pattern via GetJobByIdQuery.
		/// - This endpoint is publicly accessible unless restricted at a higher level.
		/// </remarks>
		/// <response code="200">Job retrieved successfully.</response>
		/// <response code="400">Invalid job ID supplied.</response>
		/// <response code="404">Job not found.</response>
		// GET: api/jobs/{id}
		[HttpGet("{jobid}", Name = "GetJobByJobId")]
		public async Task<IActionResult> GetJobById(string jobid)
		{
			var result = await Sender.Send(new GetJobByIdQuery(jobid));

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Creates a new job posting for the authenticated company.
		/// </summary>
		/// <param name="command">The job creation data.</param>
		/// <returns>
		/// Returns the created job identifier and details upon success.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via CreateJobCommand.
		/// - Requires Jobs_Create permission.
		/// - On success, returns a CreatedAtRoute response pointing to GetJobByJobId.
		/// </remarks>
		/// <response code="201">Job created successfully.</response>
		/// <response code="400">Invalid input data or creation failed.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		// POST /api/companiesjobs/{companyId}
		[HttpPost]
		[Authorize(Policy = Permissions.Jobs_Create)]
		public async Task<IActionResult> CreateJob(CreateJobCommand command)
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (companyId == null ) return Unauthorized();

			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);

			//return result.IsSuccess ? CreatedAtRoute("GetJobByJobId", new { id = result.Value }, result.Value  ) : BadRequest(result.Error);
		}

		/// <summary>
		/// Deletes a job posting belonging to the authenticated company.
		/// </summary>
		/// <param name="jobId">The unique identifier of the job to be deleted.</param>
		/// <returns>
		/// Returns the result of the delete operation.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Ensures that only the owning company can delete its own job postings.
		/// - Uses CQRS pattern via DeleteJobCommand.
		/// - Requires Jobs_Delete permission.
		/// </remarks>
		/// <response code="200">Job deleted successfully.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission or not job owner.</response>
		/// <response code="404">Job not found.</response>
		// DELETE: api/jobs/{jobId}
		[HttpDelete("{jobId}")]
		[Authorize(Policy = Permissions.Jobs_Delete)]
		public async Task<IActionResult> DeleteJob(string jobId)
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (companyId == null) return Unauthorized();

			var result = await Sender.Send(new DeleteJobCommand(companyId, jobId));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Publishes a job posting, making it visible to job seekers.
		/// </summary>
		/// <param name="id">The unique identifier of the job to be published.</param>
		/// <returns>
		/// Returns the result of the publish operation.
		/// </returns>
		/// <remarks>
		/// - Uses CQRS pattern via PublishJobCommand.
		/// - Typically changes the job status from Draft to Published.
		/// - May trigger downstream processes such as indexing or notifications.
		/// </remarks>
		/// <response code="200">Job published successfully.</response>
		/// <response code="400">Invalid job state or request data.</response>
		/// <response code="404">Job not found.</response>
		// POST: http://localhost:5077/api/jobs/{id}/publish
		[HttpPost("{id}/publish")]
		public async Task<IActionResult> PublishJob(string id)
		{
			var result = await Sender.Send(new PublishJobCommand(id));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Unpublishes a job posting, making it no longer visible to job seekers.
		/// </summary>
		/// <param name="id">The unique identifier of the job to be unpublished.</param>
		/// <returns>
		/// Returns the result of the unpublish operation.
		/// </returns>
		/// <remarks>
		/// - Uses CQRS pattern via UnpublishJobCommand.
		/// - Typically changes the job status from Published back to Draft or Hidden.
		/// - May affect job visibility in search results and recommendations.
		/// </remarks>
		/// <response code="200">Job unpublished successfully.</response>
		/// <response code="400">Invalid job state or request data.</response>
		/// <response code="404">Job not found.</response>
		// POST: api/jobs/{id}/unpublish
		[HttpPost("{id}/unpublish")]
		public async Task<IActionResult> UnpublishJob(string id)
		{
			var result = await Sender.Send(new UnpublishJobCommand(id));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Updates an existing job posting.
		/// </summary>
		/// <param name="id">The unique identifier of the job (from route).</param>
		/// <param name="command">The updated job data.</param>
		/// <returns>
		/// Returns the result of the update operation.
		/// </returns>
		/// <remarks>
		/// - Ensures route ID matches the request body JobId for consistency.
		/// - Uses CQRS pattern via UpdateJobCommand.
		/// - Requires Jobs_Update permission.
		/// </remarks>
		/// <response code="200">Job updated successfully.</response>
		/// <response code="400">Invalid input data or mismatched job IDs.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">Job not found.</response>
		// PUT: api/jobs/{id}
		[HttpPut("{id}")]
		[Authorize(Policy = Permissions.Jobs_Update)]
		public async Task<IActionResult> UpdateJob(string id, [FromBody] UpdateJobCommand command)
		{
			if (id != command.JobId)
				return BadRequest("Route id and body id must match");

			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}
	}
}