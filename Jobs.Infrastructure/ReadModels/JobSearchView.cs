using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.ReadModels
{
	/// <summary>
	/// Optimized read model for job search screens
	/// </summary>
	public class JobSearchView
	{
		public Guid JobId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string CompanyName { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;
		public decimal? SalaryMin { get; set; }
		public decimal? SalaryMax { get; set; }
		public bool IsRemote { get; set; }
		public DateTime PublishedAt { get; set; }
	}
}
