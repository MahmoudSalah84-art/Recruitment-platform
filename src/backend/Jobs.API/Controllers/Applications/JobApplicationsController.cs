using Jobs.API.Controllers.Abstractions;
using Jobs.API.Extensions;
using Jobs.Application.Features.Applications.Queries.GetApplicationsByJob;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Applications
{
    public class JobApplicationsController : ApiController
	{
		// GET: api/Applications/me/{CompanyId}/{jobId}
		[HttpGet]
		public async Task<IActionResult> GetAllApplicationsByJobId(string jobId, int page = 1, int pageSize = 10)
		{
			string companyId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

			var result = await Sender.Send(new GetApplicationsByJobQuery(companyId, jobId, page, pageSize));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

	}
}
