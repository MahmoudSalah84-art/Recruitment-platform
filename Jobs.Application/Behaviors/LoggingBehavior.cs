//using MediatR;
//using Microsoft.Extensions.Logging;

//namespace Jobs.Application.Behaviors
//{
//	// هذا الكلاس يراقب أي طلب (Request) وأي رد (Response) يمر عبر MediatR
//	public class LoggingBehavior<TRequest, TResponse>
//		: IPipelineBehavior<TRequest, TResponse>
//		where TRequest : IRequest<TResponse>
//		where TResponse : Result // اشترطنا هنا أن يكون الرد من نوع Result الذي شرحناه سابقاً
//	{
//		private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

//		public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
//		{
//			_logger = logger;
//		}

//		public async Task<TResponse> Handle(
//			TRequest request,
//			RequestHandlerDelegate<TResponse> next,
//			CancellationToken cancellationToken)
//		{
//			var requestName = typeof(TRequest).Name;

//			// 1. تسجيل بداية الطلب
//			_logger.LogInformation("جاري تنفيذ الطلب: {RequestName}", requestName);

//			// 2. الانتقال للخطوة التالية (سواء كانت Validation أو الـ Handler الفعلي)
//			var response = await next();

//			// 3. تسجيل نتيجة الطلب (نجاح أم فشل)
//			if (response.IsSuccess)
//			{
//				_logger.LogInformation("تم تنفيذ الطلب {RequestName} بنجاح.", requestName);
//			}
//			else
//			{
//				_logger.LogError("فشل الطلب {RequestName}. الخطأ: {Error}",
//					requestName,
//					response.Error);
//			}

//			return response;
//		}
//	}
//}
