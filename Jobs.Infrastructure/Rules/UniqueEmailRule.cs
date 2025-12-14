using System;
using System.Collections.Generic;
using System.Text;

namespace Jobs.Infrastructure.Rules
{
	/// <summary>
	/// Implementation of the domain rule that checks if an email is unique using IUserRepository.
	/// This class lives in Infrastructure because it depends on persistence.
	/// </summary>
	public class UniqueEmailRule : IUniqueEmailRule
	{
		private readonly IUserRepository _userRepository;

		public UniqueEmailRule(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<bool> IsUniqueAsync(string email)
		{
			if (string.IsNullOrWhiteSpace(email)) return false;
			return !await _userRepository.EmailExistsAsync(email);
		}
	}
}
