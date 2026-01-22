//using Microsoft.AspNetCore.Diagnostics;
//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;

//namespace Jobs.API
//{
//	// نسخة محسنة من الـ GlobalExceptionHandler مع تسجيل الأخطاء

//	public class GlobalExceptionHandler : IExceptionHandler
//	{
//		private readonly ILogger<GlobalExceptionHandler> _logger;

//		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
//		{
//			_logger = logger;
//		}

//		public async ValueTask<bool> TryHandleAsync(
//			HttpContext httpContext,
//			Exception exception,
//			CancellationToken cancellationToken)
//		{
//			// 1. تسجيل الخطأ في الـ Log للمبرمج
//			_logger.LogError(exception, "حدث خطأ غير متوقع: {Message}", exception.Message);

//			// 2. فحص نوع الخطأ: هل هو خطأ "بيانات مدخلة"؟
//			if (exception is not ValidationException validationException)
//			{
//				return false; // لو مش خطأ بيانات، سيبه للمترجمين التانيين
//			}

//			// 3. تجهيز الرد المنظم (JSON)
//			var problemDetails = new ProblemDetails
//			{
//				Status = StatusCodes.Status400BadRequest,
//				Title = "Bad Request",
//				Type = "ValidationFailure",
//				Detail = "واحد أو أكثر من الحقول المدخلة غير صحيح."
//			};

//			// 4. تجميع كل الأخطاء من كل الحقول
//			var errors = validationException.Errors
//				.GroupBy(e => e.PropertyName)
//				.ToDictionary(
//					failureGroup => failureGroup.Key,
//					failureGroup => failureGroup.Select(f => f.ErrorMessage).ToArray());

//			problemDetails.Extensions.Add("errors", errors);

//			// 5. إرسال الرد للمتصفح
//			httpContext.Response.StatusCode = problemDetails.Status.Value;
//			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

//			return true; // تم التعامل مع الخطأ بنجاح
//		}
//	}


//}
