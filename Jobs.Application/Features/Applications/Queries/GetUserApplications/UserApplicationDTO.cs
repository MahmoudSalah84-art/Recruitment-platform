using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Applications.Queries.GetUserApplications
{
    public record UserApplicationDTO
    {
		public Guid Id { get; set; }
		public Guid JobId { get; set; }
		public string JobTitle { get; set; }
		public string CompanyName { get; set; }
		public string ApplicantName { get; set; }
		public string Status { get; set; }
		public DateTime AppliedAt { get; set; }
	}
}
