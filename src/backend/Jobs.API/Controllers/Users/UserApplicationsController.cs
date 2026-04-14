using Jobs.API.Controllers.Abstractions;
using Jobs.Application.Features.Applications.Commands.SubmitApplication;
using Jobs.Application.Features.Applications.Commands.WithdrawApplication;
using Jobs.Application.Features.Applications.Queries.GetApplicationById;
using Jobs.Application.Features.Applications.Queries.GetMyApplications;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Users
{
	public class UserApplicationsController : ApiController
	{
		// POST: api/UserApplications/{jobId}
		[HttpPost("{jobId}")]
		public async Task<IActionResult> ApplyForJob(string jobId)
		{
			var result = await Sender.Send(new SubmitApplicationCommand( "ApplicantId",jobId , "cvId", 22));
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}

		// GET: api/UserApplications/{Id}
		[HttpGet("{Id}")]
		public async Task<IActionResult> GetApplicationById(string jobId)
		{
			var result = await Sender.Send(new GetUserApplicationDetailsQuery(jobId));
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}

		// GET: api/UserApplications
		[HttpGet]
		public async Task<IActionResult> GetAllApplications()
		{
			var result = await Sender.Send(new GetUserApplicationsQuery("ApplicandId"));
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}

		// DELETE: api/UserApplications/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> WithdrawApplication(string id)
		{
			var result = await Sender.Send(new WithdrawApplicationCommand(id));
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}
	}
}

//Method, Route, Description
//GET,/api/UserApplications,"عرض كل الوظائف التي تقدمت إليها وحالة كل طلب (Pending, Accepted...)."
//GET,/api/UserApplications/{id},جلب تفاصيل طلب توظيف معين ومتابعة الـ Feedback عليه.
//POST,/api/UserApplications/{jobId},التقديم على وظيفة جديدة باستخدام الـ JobId.
//DELETE,/api/UserApplications/{id},سحب طلب التقديم (Withdraw Application).

////GET / api / users / me / applications
////POST / api / jobs /{ jobId}/ apply
////DELETE / api / users / me / applications /{ id}
