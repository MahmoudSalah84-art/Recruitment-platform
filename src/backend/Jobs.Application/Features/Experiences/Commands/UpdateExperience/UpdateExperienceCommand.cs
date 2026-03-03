using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Application.Features.Experiences.Commands.UpdateExperience
{
	public class UpdateExperienceCommand : ICommand
	{
		public Guid Id { get; set; }
		public Guid CompanyId { get; set; }
		public required string JobTitle { get; set; }
		public EmploymentType EmploymentType { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public bool IsCurrent => EndDate == null;
		public string? Description { get; set; }
	}
}
