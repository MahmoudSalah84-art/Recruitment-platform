using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Users
{
	public class UserApplicationsController : ControllerBase
	{
		private readonly IMediator _mediator;
		public UserApplicationsController(IMediator mediator) => _mediator = mediator;

		[HttpPost("{jobId}")]
		public async Task<IActionResult> Apply(Guid jobId)
		{
			var result = await _mediator.Send(new ApplyToJobCommand(jobId));
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}
	}
}

//Method, Route, Description
//GET,/api/my-applications,"عرض كل الوظائف التي تقدمت إليها وحالة كل طلب (Pending, Accepted...)."
//GET,/api/my-applications/{id},جلب تفاصيل طلب توظيف معين ومتابعة الـ Feedback عليه.
//POST,/api/my-applications/{jobId},التقديم على وظيفة جديدة باستخدام الـ JobId.
//DELETE,/api/my-applications/{id},سحب طلب التقديم (Withdraw Application).