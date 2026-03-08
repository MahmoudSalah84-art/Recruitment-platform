//using Jobs.API.Controllers.Abstractions;
//using Microsoft.AspNetCore.Mvc;

//namespace Jobs.API.Controllers.Companies
//{
//	[ApiController]
//	[Route("api/companiesjobs/{companyId:guid}")]
//	public class CompanyJobsController : ApiController
//	{
//		// GET /api/companiesjobs/{companyId}
//		[HttpGet]
//		public async Task<IActionResult> GetJobs(Guid companyId)
//		{
//			var result = await Sender.Send(new GetCompanyJobsQuery(companyId));

//			return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
//		}

//		// POST /api/companiesjobs/{companyId}
//		[HttpPost]
//		public async Task<IActionResult> CreateJob( Guid companyId, CreateCompanyJobCommand command)
//		{
//			if (companyId != command.CompanyId)
//				return BadRequest("CompanyId mismatch");

//			var result = await Sender.Send(command);

//			return result.IsSuccess ? CreatedAtAction(nameof(GetJobs), new { companyId }, null) : BadRequest(result.Error);
//		}
//	}
//}
