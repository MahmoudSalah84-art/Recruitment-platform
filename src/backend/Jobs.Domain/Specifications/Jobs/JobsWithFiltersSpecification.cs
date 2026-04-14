using Jobs.Domain.Entities;
using Jobs.Domain.Enums;

namespace Jobs.Domain.Specifications.Jobs
{
	public class JobsWithFiltersSpecification : BaseSpecifications<Job>
	{
		public JobsWithFiltersSpecification(string? search, int PageSize, int PageNumber, string? Country, string? City, int? MinExperience, EmploymentType? Type,bool OnlyPublished, int? PostedInDays)
			: base(j =>
			(string.IsNullOrEmpty(search) || j.Title.Contains(search.ToLower()) || j.Description.Contains(search.ToLower())) &&

			(string.IsNullOrEmpty(Country) || j.Company.CompanyAddress.Country.Contains(Country.ToLower())) &&

			(string.IsNullOrEmpty(City) || j.Company.CompanyAddress.City.Contains(Country.ToLower())) &&

			(!MinExperience.HasValue || j.ExperienceLevel <= MinExperience) &&

			(!Type.HasValue || j.EmploymentType == Type) &&

			(j.IsPublished == OnlyPublished) &&

			(!j.IsExpired) &&

			(!PostedInDays.HasValue || j.CreatedAt >= DateTime.UtcNow.AddDays(-PostedInDays.Value))
			)
		{
			AddInclude(j => j.Company);

			ApplyPagination(PageSize * (PageNumber - 1), PageSize);

			AddOrderByDescending(j => j.CreatedAt);

		}
	}
}