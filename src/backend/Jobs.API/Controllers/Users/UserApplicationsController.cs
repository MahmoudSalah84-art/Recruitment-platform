using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Applications.Commands.SubmitApplication;
using Jobs.Application.Features.Applications.Commands.WithdrawApplication;
using Jobs.Application.Features.Applications.Queries.GetApplicationById;
using Jobs.Application.Features.Applications.Queries.GetMyApplications;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Users
{
	public class UserApplicationsController : ApiController
	{
		/// <summary>
		/// Submits a job application for the currently authenticated user.
		/// </summary>
		/// <param name="jobId">The unique identifier of the job being applied to.</param>
		/// <returns>
		/// Returns the result of the job application submission.
		/// </returns>
		/// <remarks>
		/// - The applicant is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via SubmitApplicationCommand.
		/// - Requires Applications_Apply permission.
		/// </remarks>
		/// <response code="200">Application submitted successfully.</response>
		/// <response code="400">Invalid job ID or already applied.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have permission.</response>
		// POST: api/UserApplications/{jobId}
		[HttpPost("{jobId}")]
		[Authorize(Policy = Permissions.Applications_Apply)]
		public async Task<IActionResult> ApplyForJob(string jobId)
		{
			string UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if(UserId == null) return Unauthorized();

			var result = await Sender.Send(new SubmitApplicationCommand(UserId, jobId ));

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

			var result = await Sender.Send(new GetUserApplicationDetailsQuery(companyOrUserId,applicationId));

			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Retrieves all job applications for the currently authenticated user.
		/// </summary>
		/// <returns>
		/// Returns a list of the user's job applications.
		/// </returns>
		/// <remarks>
		/// - The user is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via GetUserApplicationsQuery.
		/// - Endpoint scoped to the current user (/me).
		/// </remarks>
		/// <response code="200">Applications retrieved successfully.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		// GET: api/UserApplications
		[HttpGet("/me")]
		public async Task<IActionResult> GetAllApplicationsOfApplicant()
		{
			string applicandId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (applicandId == null)
				return Unauthorized();

			var result = await Sender.Send(new GetUserApplicationsQuery(applicandId));

			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		/// <summary>
		/// Withdraws a previously submitted job application.
		/// </summary>
		/// <param name="Applicationid">The unique identifier of the application to withdraw.</param>
		/// <returns>
		/// Returns the result of the withdrawal operation.
		/// </returns>
		/// <remarks>
		/// - The applicant is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via WithdrawApplicationCommand.
		/// - Allows users to cancel their own job applications.
		/// </remarks>
		/// <response code="200">Application withdrawn successfully.</response>
		/// <response code="400">Invalid application state or already processed.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user is not allowed to withdraw this application.</response>
		/// <response code="404">Application not found.</response>
		// DELETE: api/UserApplications/{id}
		[HttpDelete("me/Withdraw/{Applicationid}")]
		public async Task<IActionResult> WithdrawApplication(string Applicationid)
		{
			string applicandId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
			if (applicandId == null)
				return Unauthorized();

			var result = await Sender.Send(new WithdrawApplicationCommand(applicandId, Applicationid));

			var response = result.ToApiResponse<object>();
			return StatusCode(response.StatusCode, response);
		}
	}
}