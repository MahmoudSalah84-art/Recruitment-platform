namespace Jobs.API.Middlewares
{
	public class ApiResponse
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; } = string.Empty;
		public Dictionary<string, string[]>? Errors { get; set; }
		public string? ErrorCode { get; set; }
		public int StatusCode { get; set; }
		public string? TraceId { get; set; }

	}
	public class ApiResponse<T> : ApiResponse
	{
		public T? Data { get; set; }
	}
}
