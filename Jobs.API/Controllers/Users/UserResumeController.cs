using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.CV.Command.CreateOrUpdateResume;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Users
{
    public class UserResumeController : ApiController
    {
		// POST /api/userResume/me
		[HttpPost("me")]
		public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateResumeCommand command)
		{
			var result = await Sender.Send(command);

			return result.IsSuccess ? Ok() : BadRequest(result.Error);
		}

		// GET /api/userResume/me
		[HttpGet("me")]
		public async Task<IActionResult> Get()
		{
			var result = await Sender.Send(new GetMyResumeQuery());

			return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
		}

		// DELETE /api/userResume/me
		[HttpDelete("me")]
		public async Task<IActionResult> Delete()
		{
			var result = await Sender.Send(new DeleteMyResumeCommand());

			return result.IsSuccess ? NoContent() : NotFound(result.Error);
		}
	}
}

//UserResumeController,
//, POST,/api/resume, رفع ملف CV جديد.
//, DELETE,/api/resumes/{id},حذف ملف CV.
//  GET: api/UserProfile/me


//POST / api / userResume / me
//GET / api / userResume / me 
//DELETE / api / userResume / me





