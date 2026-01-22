using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Users
{
	public class UserEducationController : ControllerBase
	{
		private readonly IMediator _mediator;
		public UserEducationController(IMediator mediator) => _mediator = mediator;

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] AddEducationCommand command)
		{
			var result = await _mediator.Send(command);
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEducation(Guid id)
		{
			var result = await _mediator.Send(new DeleteEducationCommand(id));
			return result.IsSuccess ? NoContent() : BadRequest(result.Error);
		}
	}
}

//Method, Route, Description
//GET,/api/education, جلب جميع الشهادات الجامعية والدورات التدريبية.
//POST,/api/education, إضافة مؤهل تعليمي أو كورس جديد.
//PUT,/api/education/{id},تعديل بيانات مؤهل تعليمي موجود.
//DELETE,/api/education/{id},حذف مؤهل تعليمي.