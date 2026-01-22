//using Jobs.API.Controllers.Abstractions;
//using Jobs.Application.Features.Jobs.Commands.CreateJob;
//using Microsoft.AspNetCore.Mvc;

//namespace Jobs.API.Controllers
//{
//	public class JobsController : ApiController
//	{
//		[HttpPost]
//		public async Task<IActionResult> CreateJob([FromBody] CreateJobDTO request)
//		{
//			// 1. تحويل الـ Request القادم من الـ API لـ Command يفهمه الـ Application
//			var command = new CreateJobCommand(
//				request.companyId,
//				request.hrId,
//				request.title,
//				request.salary,
//				request.description,
//				request.requirements, 
//				request.expLevel,
//				request.expirationDate);
			
//			// 2. إرسال الطلب عبر الميديتر للجرسون (Mediator) ليصل للطباخ (Handler)
//			var result = await Sender.Send(command);

//			// 3. التعامل مع النتيجة بناءً على كلاس الـ Result الذي شرحناه
//			if (result.IsFailure)
//			{
//				return BadRequest(result.Error);
//			}

//			return Ok(result.Value); // يرجع الـ Id الخاص بالوظيفة الجديدة
//		}
//	}

	
//}
