using Jobs.Domain.Common;

namespace Jobs.Domain.Rules.UserExperience
{
	public class ExperienceEndDateMustBeAfterStartDateRule : IBusinessRule
	{
		private readonly DateTime _startDate;
		private readonly DateTime _endDate;

		public ExperienceEndDateMustBeAfterStartDateRule(
			DateTime startDate,
			DateTime endDate)
		{
			_startDate = startDate;
			_endDate = endDate;
		}

		public bool IsBroken()
		{
			return _endDate < _startDate;
		}

		public string Message =>
			"Experience end date must be after the start date.";
	}
}

