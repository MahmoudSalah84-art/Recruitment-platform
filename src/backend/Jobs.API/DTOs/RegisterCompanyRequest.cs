namespace Jobs.API.DTOs
{
	public class RegisterCompanyRequest
	{
		public string UserName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Industry { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string Street { get; set; } = string.Empty;
		public string BuildingNumber { get; set; } = string.Empty;
		public string PostalCode { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string Password { get; set; } = string.Empty;
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
