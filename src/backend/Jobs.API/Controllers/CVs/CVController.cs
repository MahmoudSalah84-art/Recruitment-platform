using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.CV.Query.GetMyResume;
using Jobs.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;

namespace Jobs.API.Controllers.CVs
{
    public class CVController : ApiController
    {
		//// POST /api/UserProfile/CreateOrUpdate
		//[HttpPost("CreateOrUpdate")]
		//public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateResumeCommand command)
		//{
		//	var userIdClaim = User.FindFirst("sub")?.Value;
		//	if (string.IsNullOrEmpty(userIdClaim))
		//		return Unauthorized();

		//	var result = await Sender.Send(command);

		//	return result.IsSuccess ? Ok() : BadRequest(result.Error);
		//}

		//// GET /api/userResume/cv
		//[HttpGet("cv")]
		//public async Task<IActionResult> Get()
		//{
		//	var result = await Sender.Send(new GetMyResumeQuery());

		//	return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
		//}
	}
}

//GET / api / cvs /{ id}
//POST / api / cvs
//PUT / api / cvs /{ id}
//DELETE / api / cvs /{ id}
