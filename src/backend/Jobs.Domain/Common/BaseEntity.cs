using Jobs.Domain.Exceptions;

namespace Jobs.Domain.Common
{
    public abstract class BaseEntity : SoftDelete
	{
        public string Id { get; protected set; } = Guid.NewGuid().ToString();

		public DateTime CreatedAt { get; protected set; }
		public DateTime? UpdatedAt { get; protected set; } 

		

		protected static void CheckRule(IBusinessRule rule)
		{
			if (rule.IsBroken())
				throw new BusinessRuleViolationException(rule);
		}



	}
}