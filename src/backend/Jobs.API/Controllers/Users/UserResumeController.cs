//using Jobs.API.Controllers.Abstractions;
//using Jobs.Application.Features.CV.Command.CreateOrUpdateResume;
//using Jobs.Application.Features.CV.Query.GetMyResume;
//using Microsoft.AspNetCore.Mvc;

//namespace Jobs.API.Controllers.Users
//{
//    public class UserResumeController : ApiController
//    {
//		// POST /api/userResume/me
//		[HttpPost("me")]
//		public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateResumeCommand command)
//		{
//			var result = await Sender.Send(command);

//			return result.IsSuccess ? Ok() : BadRequest(result.Error);
//		}

//		// GET /api/userResume/me
//		[HttpGet("me")]
//		public async Task<IActionResult> Get()
//		{
//			var result = await Sender.Send(new GetMyResumeQuery());

//			return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
//		}

//	}
//}






