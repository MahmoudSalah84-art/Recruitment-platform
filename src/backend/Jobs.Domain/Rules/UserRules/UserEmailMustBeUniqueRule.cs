using Jobs.Domain.Common;

namespace Jobs.Domain.Rules.UserRules
{
	public class UserEmailMustBeUniqueRule : IBusinessRule
	{
		private readonly bool _isEmailExists;

		public UserEmailMustBeUniqueRule(bool isEmailExists)
		{
			_isEmailExists = isEmailExists;
		}

		public bool IsBroken() => _isEmailExists;

		public string Message => "Email already exists.";
	}
}
