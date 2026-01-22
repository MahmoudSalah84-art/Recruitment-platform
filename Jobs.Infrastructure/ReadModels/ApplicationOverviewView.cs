using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.ReadModels
{
	/// <summary>
	/// Read model for recruiter dashboard
	/// </summary>
	public class ApplicationOverviewView
	{
		public Guid ApplicationId { get; set; }
		public string ApplicantName { get; set; } = string.Empty;
		public string JobTitle { get; set; } = string.Empty;
		public string CompanyName { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public decimal MatchScore { get; set; }
		public DateTime AppliedAt { get; set; }
	}
}
