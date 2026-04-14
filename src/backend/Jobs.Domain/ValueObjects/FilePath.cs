using Jobs.Domain.Common.YourProject.Domain.Common;
using Jobs.Domain.Exceptions;

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
				throw new DomainException("File path is required.");
			return new FilePath(path.Trim());
		}

		public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }
    }
}
