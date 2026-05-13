using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Applications.Queries.GetApplicationById;
using Jobs.Application.Features.Applications.Queries.GetApplicationsByJob;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Applications
{
    public class JobApplicationsController : ApiController
	{
		// GET: api/JobApplications/
		[HttpGet]
		public async Task<IActionResult> GetAllApplicationsByJobId(string jobId, int page = 1, int pageSize = 10)
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

			var result = await Sender.Send(new GetApplicationsByJobQuery(companyId, jobId, page, pageSize));

			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Retrieves details of a specific job application.
		/// </summary>
		/// <param name="applicationId">The unique identifier of the application.</param>
		/// <returns>
		/// Returns detailed information about the specified application.
		/// </returns>
		/// <remarks>
		/// - Uses CQRS pattern via GetUserApplicationDetailsQuery.
		/// - Requires Applications_View permission.
		/// - Access is restricted based on ownership or company role.
		/// </remarks>
		/// <response code="200">Application retrieved successfully.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - access denied.</response>
		/// <response code="404">Application not found.</response>
		// GET: api/UserApplications/{Id}
		[HttpGet("{applicationId}")]
		[Authorize(Policy = Permissions.Applications_View)]
		public async Task<IActionResult> GetApplicationById(string applicationId)
		{
			string companyOrUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (companyOrUserId == null) return Unauthorized();

			var result = await Sender.Send(new GetUserApplicationDetailsQuery(companyOrUserId, applicationId));

			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

	}
}
