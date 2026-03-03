using Jobs.Domain.Enums;

namespace Jobs.Application.Features.Jobs.Queries.GetJobsWithPagination
{
	public record JobDto 
	{
		public Guid Id { get; set; }
		public required string Title { get; set; }
		public required string Description { get; set; }

		// مثال على الـ Flattening: جلب اسم الشركة مباشرة بدلاً من كائن الشركة كاملاً
		public required string CompanyName { get; set; }
		public required string Location { get; set; }
		public EmploymentType JobType { get; set; } // part-time, full-time, contract, etc.
		public decimal? MinSalary { get; set; }
		public decimal? MaxSalary { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsExpired { get; set; } 
	}
}
