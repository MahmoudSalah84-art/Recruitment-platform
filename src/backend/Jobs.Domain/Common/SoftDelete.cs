
namespace Jobs.Domain.Common
{
	public abstract class SoftDelete
	{
		public bool IsDeleted { get; protected set; }
		public DateTime? DeletedAt { get; protected set; }

		public void MarkAsDeleted()
		{
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
		}
	}
}
