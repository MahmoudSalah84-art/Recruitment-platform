namespace Jobs.API.Middlewares
{
	public class ApiResponse<T> 
	{
		public T? Data { get; set; }
		public bool IsSuccess { get; set; }
		public string Message { get; set; } = string.Empty;
		public string? Errors { get; set; }
		public string? ErrorCode { get; set; }
		public int StatusCode { get; set; }
		public string? TraceId { get; set; }
	}
}
