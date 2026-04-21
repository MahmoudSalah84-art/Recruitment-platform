using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation;
using Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.JobRecomendation
{
	public class JobRecomendationController :ApiController
	{

		// GET   http://localhost:5077/api/job-recommendations/user/123?page=1&pageSize=10
		[HttpGet("user/{userId}")]
		public async Task<IActionResult> GetRecommendations([FromRoute] string userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
		{
			var request = new GetRecommendationByUserIdQuery(userId, page, pageSize);
			var result = await Sender.Send(request);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}



		//post
	 //  [HttpPost]
		//public async Task<IActionResult> CreateRecommendation([FromBody] CreateCVJobRecommendationCommand_cv command, CancellationToken cancellationToken)
		//{
		//	إرسال الكوماند للهاندلر
		//   var result = await Sender.Send(command, cancellationToken);

		//	إذا كنت تتبع نمط Result Pattern(وهو المفضل في Clean Architecture)
		//	return result.IsSuccess ? Ok("success") : BadRequest(result.Error);

		//	return Ok(result);
		//}


	}
}
