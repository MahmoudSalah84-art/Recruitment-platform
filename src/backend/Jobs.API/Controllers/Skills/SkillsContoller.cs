using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Skills.Commands.CreateSkill;
using Jobs.Application.Features.Skills.Commands.DeleteSkill;
using Jobs.Application.Features.Skills.Commands.RenameSkill;
using Jobs.Application.Features.Skills.Queries.GetJobsBySkill;
using Jobs.Application.Features.Skills.Queries.GetSkillById;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Skills
{
	[Route("api/skills")]
	public class SkillsController : ApiController
	{
		/// <summary>
		/// Retrieves a paginated list of skills with optional search filtering.
		/// </summary>
		/// <param name="page">Page number for pagination (default is 1).</param>
		/// <param name="pageSize">Number of items per page (default is 10).</param>
		/// <param name="search">Optional search term to filter skills by name.</param>
		/// <param name="cancellationToken">Token to cancel the request if needed.</param>
		/// <returns>
		/// Returns a paginated list of skills matching the search criteria.
		/// </returns>
		/// <remarks>
		/// - This endpoint is publicly accessible (AllowAnonymous).
		/// - Uses CQRS pattern via GetAllSkillsQuery.
		/// - Typically used for job creation or filtering job requirements.
		/// </remarks>
		/// <response code="200">Skills retrieved successfully.</response>
		/// <response code="400">Invalid pagination or query parameters.</response>
		// GET api/skills?page=1&pageSize=10&search=
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetSkills(
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10,
			[FromQuery] string? search = null,
			CancellationToken cancellationToken = default)
		{
			var query = new GetAllSkillsQuery(search, page, pageSize);

			var result = await Sender.Send(query, cancellationToken);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Retrieves a specific skill by its unique identifier.
		/// </summary>
		/// <param name="skillId">The unique identifier of the skill.</param>
		/// <param name="cancellationToken">Token to cancel the request if needed.</param>
		/// <returns>
		/// Returns the skill details if found.
		/// </returns>
		/// <remarks>
		/// - This endpoint is publicly accessible (AllowAnonymous).
		/// - Uses CQRS pattern via GetSkillByIdQuery.
		/// - Typically used when displaying skill details or job requirements.
		/// </remarks>
		/// <response code="200">Skill retrieved successfully.</response>
		/// <response code="400">Invalid skill ID supplied.</response>
		/// <response code="404">Skill not found.</response>
		// GET api/skills/{skillId}
		[HttpGet("{skillId}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetSkillById( string skillId, CancellationToken cancellationToken)
		{
			var query = new GetSkillByIdQuery(skillId);

			var result = await Sender.Send(query, cancellationToken);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Creates a new skill.
		/// </summary>
		/// <param name="request">The skill creation data.</param>
		/// <param name="cancellationToken">Token to cancel the request if needed.</param>
		/// <returns>
		/// Returns the created skill details or identifier.
		/// </returns>
		/// <remarks>
		/// - Uses CQRS pattern via CreateSkillCommand.
		/// - Typically used to add new skills to the system for job matching and profiling.
		/// - Authorization policy (Skills_Create) is recommended for securing this endpoint.
		/// </remarks>
		/// <response code="201">Skill created successfully.</response>
		/// <response code="400">Invalid input data.</response>
		/// <response code="403">Forbidden - user does not have permission.</response>
		// POST api/skills
		[HttpPost]
		//[Authorize(Policy = Permissions.Skills_Create)]
		public async Task<IActionResult> CreateSkill( [FromBody] CreateSkillCommand request, CancellationToken cancellationToken)
		{
			var result = await Sender.Send(request, cancellationToken);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Updates (renames) an existing skill.
		/// </summary>
		/// <param name="skillId">The unique identifier of the skill to be updated.</param>
		/// <param name="request">The rename skill request data.</param>
		/// <param name="cancellationToken">Token to cancel the request if needed.</param>
		/// <returns>
		/// Returns the result of the update operation.
		/// </returns>
		/// <remarks>
		/// - Uses CQRS pattern via RenameSkillCommand.
		/// - Typically used to correct or standardize skill naming.
		/// - Authorization policy (Skills_Update) is recommended for securing this endpoint.
		/// </remarks>
		/// <response code="200">Skill updated successfully.</response>
		/// <response code="400">Invalid input or skill ID mismatch.</response>
		/// <response code="404">Skill not found.</response>
		/// <response code="403">Forbidden - user does not have permission.</response>
		// PUT api/skills/{skillId}
		[HttpPut("{skillId}")]
		//[Authorize(Policy = Permissions.Skills_Update)]
		public async Task<IActionResult> UpdateSkill( string skillId, [FromBody] RenameSkillCommand request,
			CancellationToken cancellationToken)
		{
			var result = await Sender.Send(request, cancellationToken);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Deletes a skill from the system.
		/// </summary>
		/// <param name="skillId">The unique identifier of the skill to be deleted.</param>
		/// <param name="cancellationToken">Token to cancel the request if needed.</param>
		/// <returns>
		/// Returns the result of the delete operation.
		/// </returns>
		/// <remarks>
		/// - Uses CQRS pattern via DeleteSkillCommand.
		/// - This operation should typically be restricted to admin or authorized users only.
		/// - Deleting a skill may affect jobs and user profiles that reference it.
		/// </remarks>
		/// <response code="200">Skill deleted successfully.</response>
		/// <response code="400">Invalid skill ID supplied.</response>
		/// <response code="404">Skill not found.</response>
		/// <response code="403">Forbidden - user does not have permission.</response>
		// DELETE api/skills/{skillId}
		[HttpDelete("{skillId}")]
		//[Authorize(Policy = Permissions.Skills_Delete)]
		public async Task<IActionResult> DeleteSkill( string skillId, CancellationToken cancellationToken)
		{
			var command = new DeleteSkillCommand(skillId);

			var result = await Sender.Send(command, cancellationToken);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}
	}
}