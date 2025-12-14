using Jobs.Domain.Common.YourProject.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.ValueObjects
{
	public sealed class ParsedData : ValueObject
	{
		public string Json { get; }

		private ParsedData(string json) => Json = json;

		public static ParsedData Create(string json)
		{
			if (json is null) throw new ArgumentNullException(nameof(json));
			return new ParsedData(json);
		}

		public override string ToString() => Json;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
