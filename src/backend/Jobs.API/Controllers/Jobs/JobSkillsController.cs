using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Companies.Queries.GetCompanyById;
using Jobs.Application.Features.Skills.Commands.AddSkillToJob;
using Jobs.Application.Features.Skills.Commands.RemoveSkillFromJob;
using Jobs.Application.Features.Skills.Queries.GetSkillsByJob;
using Jobs.Domain.Common;
using Jobs.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Jobs
{
	[Route("api/jobs/{jobId}/skills")]
	public class JobSkillsController : ApiController
	{
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

		// POST api/jobs/{jobId}/skills
		[HttpPost]
		[Authorize(Policy = Permissions.Jobs_Update)]
		public async Task<IActionResult> AddJobSkill( string jobId, [FromBody] AddSkillToJobCommand request,
			CancellationToken cancellationToken)
		{
			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != request.CompanyId) return Unauthorized();
	
			var result = await Sender.Send(request, cancellationToken);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		// DELETE api/jobs/{jobId}/skills/{skillId}
		[HttpDelete("{skillId}")]
		public async Task<IActionResult> RemoveJobSkill(
			string jobId, string skillId, RemoveSkillFromJobCommand request,
			CancellationToken cancellationToken)
		{
			if(jobId != request.JobId || skillId != request.SkillId) return BadRequest();

			string CompanyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (CompanyId == null || CompanyId != request.CompanyId) return Unauthorized();

			var result = await Sender.Send(request, cancellationToken);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}
	}
}
