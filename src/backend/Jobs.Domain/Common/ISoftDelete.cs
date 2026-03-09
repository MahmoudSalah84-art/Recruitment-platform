
namespace Jobs.Domain.Common
{
	public interface ISoftDelete  
	{
		bool IsDeleted { get; set; }
		DateTime? DeletedAt { get;  set; }

		void SoftDelete();
		
	}
}
