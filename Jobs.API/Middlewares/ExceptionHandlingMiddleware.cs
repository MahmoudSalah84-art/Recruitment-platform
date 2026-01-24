using Jobs.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

	public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "حدث خطأ غير متوقع: {Message}", ex.Message);
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var code = HttpStatusCode.InternalServerError; // الحالة الافتراضية 500
		var result = "حدث خطأ في الخادم الداخلي.";

		// هنا السحر! نتحقق هل الخطأ من نوع BusinessException؟
		if (exception is BusinessException businessException)
		{
			code = HttpStatusCode.BadRequest; // نحول الحالة لـ 400
			result = businessException.Message; // نأخذ الرسالة التي كتبتها أنت في الكود
		}

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)code;

		// إرجاع النتيجة بشكل JSON منظم
		var response = JsonSerializer.Serialize(new { error = result, status = (int)code });

		return context.Response.WriteAsync(response);
	}
}