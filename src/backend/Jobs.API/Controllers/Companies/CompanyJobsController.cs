using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Jobs.Queries.GetJobsByCompany;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Companies
{
	public class CompanyJobsController : ApiController
	{
		/// <summary>
		/// Retrieves all jobs for the currently authenticated company.
		/// </summary>
		/// <returns>
		/// Returns a list of jobs associated with the authenticated company.
		/// </returns>
		/// <remarks>
		/// - The company is identified using the authenticated user's claims (NameIdentifier).
		/// - Uses CQRS pattern via GetJobsByCompanyQuery.
		/// - Requires Jobs_View permission.
		/// </remarks>
		/// <response code="200">Jobs retrieved successfully.</response>
		/// <response code="401">Unauthorized - user is not authenticated.</response>
		/// <response code="403">Forbidden - user does not have required permission.</response>
		/// <response code="404">Company not found.</response>
		// GET /api/companiesjobs/{companyId}
		[HttpGet]
		[Authorize(Policy = Permissions.Jobs_View)]
		public async Task<IActionResult> GetJobs(int Page , int PageSize )
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

			var result = await Sender.Send(new GetJobsByCompanyQuery(companyId, Page, PageSize));

			var response = result.ToApiResponse();

			return StatusCode(response.StatusCode, response);
		}
	}
}