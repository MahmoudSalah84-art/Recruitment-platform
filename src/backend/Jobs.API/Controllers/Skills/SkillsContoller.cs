using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Skills.Commands.CreateSkill;
using Jobs.Application.Features.Skills.Commands.DeleteSkill;
using Jobs.Application.Features.Skills.Commands.RenameSkill;
using Jobs.Application.Features.Skills.Queries.GetJobsBySkill;
using Jobs.Application.Features.Skills.Queries.GetSkillById;
using Jobs.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Skills
{
	[Route("api/skills")]
	public class SkillsController : ApiController
	{
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

		// GET api/skills/{skillId}
		[HttpGet("{skillId}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetSkillById(
			string skillId,
			CancellationToken cancellationToken)
		{
			var query = new GetSkillByIdQuery(skillId);

			var result = await Sender.Send(query, cancellationToken);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		// POST api/skills
		[HttpPost]
		//[Authorize(Policy = Permissions.Skills_Create)]
		public async Task<IActionResult> CreateSkill( [FromBody] CreateSkillCommand request,
			CancellationToken cancellationToken)
		{
			var result = await Sender.Send(request, cancellationToken);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}

		// PUT api/skills/{skillId}
		[HttpPut("{skillId}")]
		//[Authorize(Policy = Permissions.Skills_Update)]
		public async Task<IActionResult> UpdateSkill(
			string skillId, [FromBody] RenameSkillCommand request,
			CancellationToken cancellationToken)
		{
			var result = await Sender.Send(request, cancellationToken);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

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
