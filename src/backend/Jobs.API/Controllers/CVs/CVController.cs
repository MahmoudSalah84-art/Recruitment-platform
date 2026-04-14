using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.CV.Command.CreateOrUpdateResume;
using Jobs.Application.Features.CV.Query.GetMyResume;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.CVs
{
	public class CVController : ApiController
	{
		// POST /api/UserProfile/CreateOrUpdate
		[HttpPost("CreateOrUpdate")]
		public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateResumeCommand command)
		{
			var result = await Sender.Send(command);

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		// GET /api/userResume/cv
		[HttpGet("cv")]
		public async Task<IActionResult> Get(GetMyResumeQuery Query)
		{
			var result = await Sender.Send(Query);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}
	}
}