using Jobs.Application.Abstractions.Messaging;
using Jobs.Domain.Enums;
using MediatR;

namespace Jobs.Application.Features.Experiences.Commands.AddUserExperience
{
	public class AddExperienceCommand :  ICommand<Guid>
	{
		public Guid CompanyId { get; set; }
		public required string JobTitle { get; set; }
		public EmploymentType EmploymentType { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public bool IsCurrentRole => EndDate == null;
		public string? Description { get; set; }
	}
}
