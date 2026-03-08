using Jobs.Domain.Common.YourProject.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Domain.ValueObjects
{
	public sealed class FilePath : ValueObject
	{
		public string Value { get; }

		public FilePath() { }

		private FilePath(string path) => Value = path;

		public static FilePath Create(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new ArgumentException("File path is required.", nameof(path));
			return new FilePath(path.Trim());
		}

		public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
