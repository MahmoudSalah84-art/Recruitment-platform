using Jobs.Application.Common.Exceptions;
using Jobs.Domain.Exceptions;
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
			_logger.LogError(ex, "unexpected error: {Message}", ex.Message);
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var code = HttpStatusCode.InternalServerError;
		var result = "unexpected error occurred in the server, please try again later.";

		if (exception is BusinessRuleViolationException businessRule)
		{
			code = HttpStatusCode.BadRequest; 
			result = businessRule.Message;
		}

		context.Response.ContentType = "application/json"; 
		context.Response.StatusCode = (int)code;
		
		var response = JsonSerializer.Serialize(new { error = result, status = (int)code });

		return context.Response.WriteAsync(response);
	}
}