
using Jobs.Domain.Common;

namespace Jobs.Domain.Rules.CompanyRoles
{
    public class CompanyNameMustBeUniqueRule : IBusinessRule
	{
		private readonly bool _isNameExists;

		public CompanyNameMustBeUniqueRule(bool isNameExists)
		{
			_isNameExists = isNameExists;
		}
		public bool IsBroken() => _isNameExists;

		public string Message => "Name already exists.";
	}
	
}
