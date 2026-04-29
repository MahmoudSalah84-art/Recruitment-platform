using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Skills.Commands.AddUserSkill;
using Jobs.Application.Features.Skills.Commands.DeleteUserSkill;
using Jobs.Application.Features.Skills.Queries.GetUserSkills;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Users
{
	public class UserSkillsController : ApiController
	{
		/// <summary>
		/// Retrieves all skills for the currently authenticated user.
		/// </summary>
		/// <returns>
		/// Returns a list of skills associated with the user.
		/// </returns>
		/// <remarks>
		/// - The user is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via GetUserSkillsQuery.
		/// </remarks>
		/// <response code="200">User skills retrieved successfully.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		// GET /api/userSkills
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var result = await Sender.Send(new GetUserSkillsQuery());

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Adds a skill to the currently authenticated user.
		/// </summary>
		/// <param name="skillid">The unique identifier of the skill to be added.</param>
		/// <returns>
		/// Returns the result of the add operation.
		/// </returns>
		/// <remarks>
		/// - The user is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via AddUserSkillCommand.
		/// - Prevents duplicate skill assignments (recommended at domain level).
		/// </remarks>
		/// <response code="200">Skill added successfully.</response>
		/// <response code="400">Invalid or duplicate skill.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		// POST /api/userSkills
		[HttpPost("{skillid}")]
		public async Task<IActionResult> Add(string skillid)
		{
			string seekerId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (seekerId == null) return Unauthorized();

			var result = await Sender.Send(new AddUserSkillCommand(seekerId, skillid));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);

		}

		/// <summary>
		/// Removes a skill from the currently authenticated user.
		/// </summary>
		/// <param name="skillid">The unique identifier of the skill to be removed.</param>
		/// <returns>
		/// Returns the result of the delete operation.
		/// </returns>
		/// <remarks>
		/// - The user is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via DeleteUserSkillCommand.
		/// </remarks>
		/// <response code="200">Skill removed successfully.</response>
		/// <response code="400">Invalid skill ID.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		// DELETE /api/userSkills/{skillid}
		[HttpDelete("{skillid}")]
		public async Task<IActionResult> Delete(string skillid)
		{
			string seekerId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (seekerId == null) return Unauthorized();

			var result = await Sender.Send(new DeleteUserSkillCommand(seekerId,skillid));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}
	}
}