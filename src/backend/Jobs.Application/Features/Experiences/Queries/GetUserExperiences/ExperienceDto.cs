namespace Jobs.Application.Features.Experiences.Queries.GetUserExperiences
{
    public record ExperienceDto
    {
		public Guid Id { get; set; }
		public required string CompanyName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public bool IsCurrentRole { get; set; }
		public required string EmploymentType { get; set; }
		public required string JobTitle { get; set; }
		public string? Description { get; set; }
	}
}
