using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Applications.Commands.UpdateApplicationStatus;
using Jobs.Application.Features.Applications.Queries.GetApplicationById;
using Jobs.Application.Features.Applications.Queries.GetApplicationsByJob;
using Jobs.Domain.Common;
using Jobs.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Applications
{
    public class JobApplicationsController : ApiController
	{
		// GET: api/JobApplications/
		[HttpGet]
		[Authorize(Policy = Permissions.Applications_View)]
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
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

			var result = await Sender.Send(new GetUserApplicationDetailsQuery(companyId, applicationId));

			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}







		// API/Controllers/JobApplicationsController.cs
		[HttpPatch("{applicationId}/status")]
		[Authorize(Policy = Permissions.Applications_UpdateStatus)] 
		public async Task<IActionResult> UpdateStatus(
			string applicationId,
			[FromBody] ApplicationStatus NewStatus,
			CancellationToken cancellationToken)
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


			var command = new UpdateApplicationStatusCommand(companyId, applicationId, NewStatus);
			var result = await Sender.Send(command, cancellationToken);

			var response = result.ToApiResponse<object>();
			return StatusCode(response.StatusCode, response);
		}

	}
}
