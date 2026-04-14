//using Jobs.API.Controllers.Abstractions;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace Jobs.API.Controllers.Companies
//{
//	[Authorize(Roles = "Company")]
//	public class CompanyStatisticsController : ApiController
//	{
//		//GET /api/companies/{companyId}/statistics
//		[HttpGet]
//		public async Task<IActionResult> GetStatistics()
//		{
//			var companyId = User.FindFirst("sub")?.Value;

//			var result = await _mediator.Send(new GetCompanyStatisticsQuery(companyId));

//			return Ok(result);
//		}
//	}
//}



//عدد المتقدمين على الوظائف
//عدد الـ Applications لكل Job
//عدد المقبولين / المرفوضين
//عدد الوظائف النشطة vs المغلقة
//آخر نشاط حصل