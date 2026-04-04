using FluentValidation;
using Jobs.Domain.Exceptions;
using Jobs.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Jobs.API.Middlewares
{
	public class GlobalExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<GlobalExceptionHandler> _logger;

		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
		{
			_logger = logger;
		}

		public async ValueTask<bool> TryHandleAsync( HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
		{
			_logger.LogError(ex, "Unexpected Error: {Message}", ex.Message);

			var traceId = httpContext.TraceIdentifier;

			var response = new ApiResponse
			{
				IsSuccess = false,
				TraceId = traceId
			};

			switch (ex)
			{
				case ValidationException validationEx:
					response.Message = validationEx.Message;
					response.Errors = validationEx.Errors
							.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
							.ToDictionary(
								failureGroup => failureGroup.Key,
								failureGroup => failureGroup.ToArray()
							);
					response.ErrorCode = ErrorCodes.Validation;
					response.StatusCode = StatusCodes.Status400BadRequest;
					break;

				case DatabaseException:
					response.Message = "Database error occurred";
					response.ErrorCode = ErrorCodes.ServerError;
					response.StatusCode = StatusCodes.Status500InternalServerError;
					break;

				case DomainException or BusinessRuleViolationException:
					response.Message = ex.Message;
					response.ErrorCode = ErrorCodes.businessRule;
					response.StatusCode = StatusCodes.Status400BadRequest;
					break;

				//case NotFoundException:
				//	response.Message = ex.Message;
				//	response.ErrorCode = ErrorCodes.NotFound;
				//	response.StatusCode = StatusCodes.Status404NotFound;
				//	break;

				//case UnauthorizedException:
				//	response.Message = ex.Message;
				//	response.ErrorCode = ErrorCodes.Unauthorized;
				//	response.StatusCode = StatusCodes.Status401Unauthorized;
				//	break;

				//case ConflictException:
				//	response.Message = ex.Message;
				//	response.ErrorCode = ErrorCodes.Conflict;
				//	response.StatusCode = StatusCodes.Status409Conflict;
				//	break;

				default:
					response.Message = $"Internal Server Error : ({ex.Message}) ";
					response.ErrorCode = ErrorCodes.ServerError;
					response.StatusCode = StatusCodes.Status500InternalServerError;
					break;
			}

			httpContext.Response.StatusCode = response.StatusCode;
			httpContext.Response.ContentType = "application/json";

			await httpContext.Response.WriteAsJsonAsync(response);

			return true;
		}
	}
}
