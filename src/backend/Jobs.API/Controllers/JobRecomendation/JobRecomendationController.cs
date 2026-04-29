using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.CVJobRecommendation.Command.CreateCVJobRecommendation;
using Jobs.Application.Features.CVJobRecommendation.Queries.GetRecommendationByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.JobRecomendation
{
	public class JobRecomendationController :ApiController
	{
		/// <summary>
		/// Retrieves job recommendations for the currently authenticated user.
		/// </summary>
		/// <param name="page">Page number for pagination (default is 1).</param>
		/// <param name="pageSize">Number of items per page (default is 10).</param>
		/// <returns>
		/// Returns a paginated list of recommended jobs for the user.
		/// </returns>
		/// <remarks>
		/// - The user is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via GetRecommendationByUserIdQuery.
		/// - Recommendations are typically based on user profile, CV, and preferences.
		/// </remarks>
		/// <response code="200">Recommendations retrieved successfully.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">User not found.</response>
		// GET   http://localhost:5077/api/job-recommendations/user/123?page=1&pageSize=10
		[HttpGet("user/{userId}")]
		public async Task<IActionResult> GetRecommendations( [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
		{
			string seekerId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (seekerId == null) return Unauthorized();

			var request = new GetRecommendationByUserIdQuery(seekerId, page, pageSize);
			var result = await Sender.Send(request);

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}


		//for testing purpose only
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
