using Jobs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.Rules.UserExperience
{
	public class ExperienceDateRangeRule : IBusinessRule
	{
		private readonly DateTime _start;
		private readonly DateTime? _end;
		private readonly bool _isCurrent;

		public ExperienceDateRangeRule(DateTime start, DateTime? end, bool isCurrent)
		{
			_start = start;
			_end = end;
			_isCurrent = isCurrent;
		}

		public bool IsBroken()
		{
			if (_isCurrent && _end.HasValue)
				return true;

			if (!_isCurrent && !_end.HasValue)
				return true;

			if (_end.HasValue && _end < _start)
				return true;

			return false;
		}

		public string Message =>
			"Invalid experience date range.";
	}

}
