using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Jobs.Commands.CreateJob;
using Jobs.Application.Features.Jobs.Queries.GetJobsByCompany;
using Jobs.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jobs.API.Controllers.Companies
{
	public class CompanyJobsController : ApiController
	{
	
		// GET /api/companiesjobs/{companyId}
		[HttpGet]
		[Authorize(Policy = Permissions.Jobs_View)]
		public async Task<IActionResult> GetJobs(string companyId)
		{


			var result = await Sender.Send(new GetJobsByCompanyQuery(companyId));

			return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
		}

	}
}