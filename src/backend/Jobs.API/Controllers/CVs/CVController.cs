using Jobs.API.Controllers.Abstractions;
using Jobs.API.DTOs;
using Jobs.API.Extensions;
using Jobs.Application.Common.DTOs;
using Jobs.Application.Features.CV.Command.CreateOrUpdateResume;
using Jobs.Application.Features.CV.Query.GetMyResume;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.CVs
{
	public class CVController : ApiController
	{
		// POST /api/UserProfile/CreateOrUpdate
		[HttpPost()]
		[Authorize(Policy = Permissions.CVs_Upload)]
		public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateResumeDto command)
		{

			string seekerId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (seekerId == null || seekerId != command.UserId) return Unauthorized();

			var fileUploadDto = new FileUploadDto(command.File.FileName, command.File.ContentType, command.File.OpenReadStream());

			var result = await Sender.Send(new CreateOrUpdateResumeCommand(command.UserId, fileUploadDto));

			var response = result.ToApiResponse<object>();

			return StatusCode(response.StatusCode, response);
		}

		// GET http://localhost:5077/api/cv/userId=1
		[HttpGet()]
		[Authorize(Policy = Permissions.CVs_View)]
		public async Task<IActionResult> GetCv( [FromQuery] GetMyResumeQuery Query)
		{
			string seekerId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (seekerId == null || seekerId != Query.userId) return Unauthorized();

			var result = await Sender.Send(Query);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}
	}
}