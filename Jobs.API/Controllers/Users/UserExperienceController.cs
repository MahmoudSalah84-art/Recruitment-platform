using Jobs.API.Controllers.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Users
{
	public class UserExperienceController : ApiController
	{
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] AddExperienceCommand command)
		{
			var result = await Sender.Send(command);
			return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
		}

		
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var result = await Sender.Send(new DeleteExperienceCommand(id));
			return result.IsSuccess ? NoContent() : BadRequest(result.Error);
		}


	}
}


//Method, Route, Description
//GET,/api/experiences, جلب قائمة بكل الخبرات الوظيفية للمستخدم الحالي.
//POST,/api/experiences, إضافة خبرة وظيفية جديدة (شركة، منصب، تاريخ).
//PUT,/api/experiences/{id},تحديث بيانات خبرة سابقة بواسطة الـ ID الخاص بها.
//DELETE,/api/experiences/{id},حذف خبرة وظيفية من الملف الشخصي.