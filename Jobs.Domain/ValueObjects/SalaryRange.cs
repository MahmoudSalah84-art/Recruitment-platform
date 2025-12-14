using Jobs.Domain.Common.YourProject.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.ValueObjects
{
	public sealed class SalaryRange : ValueObject
	{
		public decimal? Min { get; }
		public decimal? Max { get; }

		private SalaryRange(decimal? min, decimal? max)
		{
			Min = min;
			Max = max;
		}

		public static SalaryRange Create(decimal? min, decimal? max)
		{
			if (min.HasValue && max.HasValue && max < min)
				throw new ArgumentException("Max salary cannot be less than Min salary.");
			return new SalaryRange(min, max);
		}

		public override string ToString() => $"{Min?.ToString() ?? "-"} - {Max?.ToString() ?? "-"}";

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
