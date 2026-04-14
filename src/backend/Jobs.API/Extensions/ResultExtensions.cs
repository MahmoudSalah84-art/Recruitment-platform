using Jobs.API.Middlewares;
using Jobs.Application.Abstractions.Messaging;


namespace Jobs.API.Extensions
{
	public static class ResultExtensions
	{
		public static ApiResponse<T> ToApiResponse<T>(this Result<T> result, int successStatusCode = 200)
		{
			if (result.IsSuccess)
			{
				return new ApiResponse<T>
				{
					IsSuccess = true,
					Message = "Success",
					Data = result.Value,
					StatusCode = successStatusCode
				};
			}

			return new ApiResponse<T>
			{
				IsSuccess = false,
				Message = "Faild" ,
				Errors = result.Error,
				StatusCode = 400
			};
		}

		public static ApiResponse<T> ToApiResponse<T>(this Result result, int successStatusCode = 200)
		{
			if (result.IsSuccess)
			{
				return new ApiResponse<T>
				{
					IsSuccess = true,
					Message = "Success",
					StatusCode = successStatusCode
				};
			}

			return new ApiResponse<T>
			{
				IsSuccess = false,
				Message = "Faild" ,
				Errors = result.Error,
				StatusCode = 400
			};
		}

	}
}
