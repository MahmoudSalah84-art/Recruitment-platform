using Jobs.API.Middlewares;
using Jobs.Application.Abstractions.Messaging;

namespace Jobs.API.Extensions
{
	/// <summary>
	/// Extension helpers to convert Result and Result&lt;T&gt; values into ApiResponse&lt;T&gt;.
	/// </summary>
	public static class ResultExtensions
	{
		/// <summary>
		/// Converts a <see cref="Result{T}"/> to an <see cref="ApiResponse{T}"/>.
		/// </summary>
		/// <typeparam name="T">Type of the response data.</typeparam>
		/// <param name="result">The result to convert.</param>
		/// <param name="successStatusCode">HTTP status code to set when the result is successful. Default is 200.</param>
		/// <returns>An <see cref="ApiResponse{T}"/> representing the provided result.</returns>
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

		/// <summary>
		/// Converts a non-generic <see cref="Result"/> to an <see cref="ApiResponse{T}"/>.
		/// </summary>
		/// <typeparam name="T">Type parameter for the returned <see cref="ApiResponse{T}"/> (may be null for no data).</typeparam>
		/// <param name="result">The result to convert.</param>
		/// <param name="successStatusCode">HTTP status code to set when the result is successful. Default is 200.</param>
		/// <returns>An <see cref="ApiResponse{T}"/> representing the provided result.</returns>
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
				Message = "Faild",
				Errors = result.Error,
				StatusCode = 400
			};
		}

	}
}
