using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobs.API.Controllers.Abstractions
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class ApiController : ControllerBase
	{
		private ISender? _sender;

		// خاصية تجلب الـ Sender تلقائياً عند أول استخدام
		protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
	}
}
