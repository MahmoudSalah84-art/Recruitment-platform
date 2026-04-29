using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Skills.Commands.AddSkillToJob;
using Jobs.Application.Features.Skills.Commands.RemoveSkillFromJob;
using Jobs.Application.Features.Skills.Queries.GetSkillsByJob;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Jobs
{
	[Route("api/jobs/{jobId}/skills")]
	public class JobSkillsController : ApiController
	{
		/// <summary>
		/// Retrieves the skills required for a specific job.
		/// </summary>
		/// <param name="jobId">The unique identifier of the job.</param>
		/// <param name="cancellationToken">Token to cancel the request if needed.</param>
		/// <returns>
		/// Returns a list of skills associated with the specified job.
		/// </returns>
		/// <remarks>
		/// - Uses CQRS pattern via GetJobSkillsQuery.
		/// - Requires Skills_View permission.
		/// - Typically used to display required skills in job details.
		/// </remarks>
		/// <response code="200">Job skills retrieved successfully.</response>
		/// <response code="400">Invalid job ID supplied.</response>
		/// <response code="404">Job not found.</response>
		// GET api/jobs/{jobId}/skills
		[HttpGet]
		[Authorize(Policy = Permissions.Skills_View)]
		public async Task<IActionResult> GetJobSkills(string jobId, CancellationToken cancellationToken)
		{
			var query = new GetJobSkillsQuery(jobId);

			var result = await Sender.Send(query, cancellationToken);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Adds a skill to a specific job posting.
		/// </summary>
		/// <param name="jobId">The unique identifier of the job.</param>
		/// <param name="skillId">The unique identifier of the skill to be added.</param>
		/// <param name="cancellationToken">Token to cancel the request if needed.</param>
		/// <returns>
		/// Returns the result of the add operation.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via AddSkillToJobCommand.
		/// - Requires Jobs_Update permission.
		/// - Ensures that only the owning company can modify its job skills.
		/// </remarks>
		/// <response code="200">Skill added to job successfully.</response>
		/// <response code="400">Invalid job or skill ID supplied.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission or not job owner.</response>
		/// <response code="404">Job or skill not found.</response>
		// POST api/jobs/{jobId}/skills
		[HttpPost]
		[Authorize(Policy = Permissions.Jobs_Update)]
		public async Task<IActionResult> AddJobSkill( string jobId,string skillId, CancellationToken cancellationToken)
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (companyId == null ) return Unauthorized();
	
			var result = await Sender.Send(new AddSkillToJobCommand( jobId, skillId,companyId), cancellationToken);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Removes a skill from a specific job posting.
		/// </summary>
		/// <param name="jobId">The unique identifier of the job.</param>
		/// <param name="skillId">The unique identifier of the skill to be removed.</param>
		/// <param name="cancellationToken">Token to cancel the request if needed.</param>
		/// <returns>
		/// Returns the result of the remove operation.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via RemoveSkillFromJobCommand.
		/// - Requires Jobs_Update permission (recommended at controller or policy level).
		/// - Ensures only the owning company can modify its job skills.
		/// </remarks>
		/// <response code="200">Skill removed from job successfully.</response>
		/// <response code="400">Invalid job or skill ID supplied.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission or not job owner.</response>
		/// <response code="404">Job or skill not found.</response>
		// DELETE api/jobs/{jobId}/skills/{skillId}
		[HttpDelete("{skillId}")]
		public async Task<IActionResult> RemoveJobSkill( string jobId, string skillId, CancellationToken cancellationToken)
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (companyId == null ) return Unauthorized();

			var result = await Sender.Send(new RemoveSkillFromJobCommand(jobId, skillId, companyId), cancellationToken);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}
	}
}
